using System;
using System.Collections.Generic;
using NUnit.Framework;
using RecipesApp.Core.Domain;
using RecipesApp.Core.Services;

namespace RecipesApp.Tests
{
    [TestFixture]
    public class RecipeServiceTests
    {
        private FakeRepository<Recipe> _fakeRepo;
        private RecipeService _recipeService;

        [SetUp]
        public void SetUp()
        {
            _fakeRepo = new FakeRepository<Recipe>();
            _recipeService = new RecipeService(_fakeRepo);
        }


        [Test]
        public void SearchRecipes_WhenQueryMatchesTitle_ReturnsMatchedRecipes()
        {
            _fakeRepo.Items.Add(new Recipe { Title = "Борщ", Description = "Український суп" });
            _fakeRepo.Items.Add(new Recipe { Title = "Піца", Description = "Італійська страва" });

            var result = _recipeService.SearchRecipes("борщ");

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Title, Is.EqualTo("Борщ"));
        }

        [Test]
        public void ToggleFavorite_WhenRecipeExists_TogglesStatus()
        {
            var recipe = new Recipe { Id = "1", IsFavorite = false };
            _fakeRepo.Items.Add(recipe);

            _recipeService.ToggleFavorite("1");

            var updatedRecipe = _fakeRepo.GetById("1");
            Assert.That(updatedRecipe.IsFavorite, Is.True); 
        }

        [Test]
        public void ScaleRecipe_WithValidFactor_ScalesIngredientsCorrectly()
        {
            var original = new Recipe
            {
                Title = "Млинці",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient("Борошно", new Quantity(200, MeasurementUnit.Gram))
                }
            };

            var scaled = _recipeService.ScaleRecipe(original, 2.5);

            Assert.That(scaled.Title, Is.EqualTo("Млинці (Масштабований x2.5)"));
            Assert.That(scaled.Ingredients[0].Amount.Value, Is.EqualTo(500)); 
        }



        [Test]
        public void ToggleFavorite_WhenRecipeDoesNotExist_ThrowsKeyNotFoundException()
        {

            var ex = Assert.Throws<KeyNotFoundException>(() => _recipeService.ToggleFavorite("invalid_id"));
            Assert.That(ex.Message, Does.Contain("не знайдено"));
        }

        [Test]
        public void UpdateNotes_WhenRecipeDoesNotExist_ThrowsKeyNotFoundException()
        {
            Assert.Throws<KeyNotFoundException>(() => _recipeService.UpdateNotes("999", "Нова нотатка"));
        }



        [Test]
        public void ScaleRecipe_WithZeroOrNegativeFactor_ThrowsArgumentException()
        {
            var original = new Recipe { Title = "Млинці" };

            Assert.Throws<ArgumentException>(() => _recipeService.ScaleRecipe(original, 0));
            Assert.Throws<ArgumentException>(() => _recipeService.ScaleRecipe(original, -1.5));
        }

        [Test]
        public void SearchRecipes_WithEmptyQuery_ReturnsAllRecipes()
        {
            _fakeRepo.Items.Add(new Recipe { Title = "Борщ" });
            _fakeRepo.Items.Add(new Recipe { Title = "Піца" });

            var result = _recipeService.SearchRecipes(""); 

            Assert.That(result.Count, Is.EqualTo(2));
        }
    }
}
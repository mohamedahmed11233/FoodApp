using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum.SharedEnums
{
    public enum ErrorCode
    {
        // General
        None = 0,
        InvalidInput = 3,
        NotAuthorized = 4,

        // Recipe
        FailedAddRecipe = 100,
        FailedUpdateRecipe = 101,
        FailedDeleteRecipe = 102,
        RecipeNotFound = 103,
        InvalidRecipeData = 104,

        // User
        FailedRegister = 200,
        FailedLogin = 201,
        FailedProfileUpdate = 205,
        UserNotFound = 207,
        InvalidCredentials = 208,

        // Favorites
        FailedAddToFavorites = 300,
        FailedRemoveFromFavorites = 301,

        // Search
        SearchFailed = 400,

        // Payment
        PaymentFailure = 1000,

        // External Services
        ExternalServiceUnavailable = 2000
    }

}

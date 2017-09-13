angular.module('iClipServices')
    .constant('urlsList', {

        //#region Accounts

        // Url which is for obtain personal profile.
        getPersonalProfile: 'api/account/personal-profile',

        // Url which is for logging using basic account information.
        internalLogin: 'api/account/internal-login',

        // Url which is for logging into system using external account.
        externalLogin: 'api/account/external-login',

        // Url which is for registering an account using basic information/
        basicProfileRegistration: 'api/account/internal-registration',

        //#endregion

        //#region Categories

        // Url which is for searching categories by using specific conditions.
        getCategories: 'api/category/search',

        // Url which is for initiating category.
        initCategory: 'api/category',

        // Url which is for editing category.
        editCategory: 'api/category'

        //#endregion
    });
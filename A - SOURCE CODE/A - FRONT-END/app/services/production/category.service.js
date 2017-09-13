angular.module('iClipServices')
    .service('categoryService', ['baseUrl', 'urlsList', '$http',
        function (baseUrl, urlsList, $http) {

            //#region Methods

            /*
            * Get list of categories by using specific conditions.
            * */
            this.getCategories = function (condition) {
                let url = baseUrl + '/' + urlsList.getCategories;
                return $http.post(url, condition);
            };

            /*
            * Init category by using specific condition.
            * */
            this.initCategory = function (category) {
                let url = baseUrl + '/' + urlsList.initCategory;
                return $http.post(url, category);
            };

            /*
            * Edit category using specific information.
            * */
            this.editCategory = function (categoryId, category) {
                let url = baseUrl + '/' + urlsList.editCategory;
                let params = {categoryId: categoryId};
                return $http.put(url, category, params);
            };

            //#endregion
        }]);
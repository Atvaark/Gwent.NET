(function () {
    'use strict';

    angular.module('app.game')
        .directive('gwCardBig', function () {
            return {
                restrict: 'A',
                require: '^ngModel',
                scope: {
                    card: '=ngModel'
                },
                templateUrl: 'templates/game/card-big.html'
            };
        });
})();
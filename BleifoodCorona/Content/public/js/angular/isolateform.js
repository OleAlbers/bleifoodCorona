/**
 * Created by Tomato on 01.05.2015.
 * geklaut von SO: http://stackoverflow.com/questions/19333544/skip-nested-forms-validation-with-angularjs/24936234#24936234
 *
 */




foodtruckApp.directive('isolateForm', [function () {
    return {
        restrict: 'A',
        require: '?form',
        link: function (scope, elm, attrs, ctrl) {
            if (!ctrl) {
                return;
            }

            // Do a copy of the controller
            var ctrlCopy = {};
            angular.copy(ctrl, ctrlCopy);

            // Get the parent of the form
            var parent = elm.parent().controller('form');
            // Remove parent link to the controller
            parent.$removeControl(ctrl);

            // Replace form controller with a "isolated form"
            var isolatedFormCtrl = {
                $setValidity: function (validationToken, isValid, control) {
                    ctrlCopy.$setValidity(validationToken, isValid, control);
                    parent.$setValidity(validationToken, true, ctrl);
                },
                $setDirty: function () {
                    elm.removeClass('ng-pristine').addClass('ng-dirty');
                    ctrl.$dirty = true;
                    ctrl.$pristine = false;
                }
            };
            angular.extend(ctrl, isolatedFormCtrl);
        }
    };
}]);

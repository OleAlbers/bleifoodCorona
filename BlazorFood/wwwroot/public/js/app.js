/**
 * Created by Tomato on 25.05.2015.
 */

var foodtruckApp=angular.module('foodtruckApp',[
    'ui.bootstrap',
    'frapontillo.bootstrap-switch',
    'angulartics',
    'angulartics.google.analytics',
    'ngDragDrop','uiGmapgoogle-maps',
    'ui.sortable',
    'ngImgCrop',
    'angularUtils.directives.dirDisqus' // DisQus
]);



foodtruckApp.directive('httpPrefix', function() {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function(scope, element, attrs, controller) {
            function ensureHttpPrefix(value) {
                // Need to add prefix if we don't have http:// prefix already AND we don't have part of it
                if(value && !/^(https?):\/\//i.test(value)
                    && 'http://'.indexOf(value) === -1) {
                    controller.$setViewValue('http://' + value);
                    controller.$render();
                    return 'http://' + value;
                }
                else
                    return value;
            }
            controller.$formatters.push(ensureHttpPrefix);
            controller.$parsers.splice(0, 0, ensureHttpPrefix);
        }
    };
});



foodtruckApp.filter('to_trusted', ['$sce', function($sce){
    return function(text) {
        return $sce.trustAsHtml(text);
    };
}]);


foodtruckApp.config(function (datepickerConfig, datepickerPopupConfig) {
    datepickerConfig.startingDay = 1;
    datepickerConfig.showWeeks = false;


    datepickerPopupConfig.currentText="Heute";
    datepickerPopupConfig.closeText="Fertig";
    datepickerPopupConfig.clearText="Abbrechen";
    datepickerPopupConfig.datepickerPopup="dd.MM.yyyy";
    datepickerPopupConfig.showButtonBar=false;

});






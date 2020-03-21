/**
 * Created by lbo on 16.06.15.
 */
//var foodtruckFrontendApp=angular.module('foodtruckFrontendApp',['ui.bootstrap', 'angulartics','angulartics.google.analytics','uiGmapgoogle-maps']);
var foodtruckFrontendApp=angular.module('foodtruckFrontendApp',[
    'ui.bootstrap',
    'frapontillo.bootstrap-switch',
    'angulartics',
    'angulartics.google.analytics',
    'angularLoad',
    'uiGmapgoogle-maps',
    'viewhead', // <Title> und OG-Tags setzen
    'angularUtils.directives.dirDisqus' // DisQus
]);

foodtruckFrontendApp.filter('to_trusted', ['$sce', function($sce){
    return function(text) {
        return $sce.trustAsHtml(text);
    };
}]);

foodtruckFrontendApp.config(['$locationProvider', function AppConfig( $locationProvider) {
    // enable html5Mode for pushstate ('#'-less URLs)
    $locationProvider.html5Mode(true);
//    $locationProvider.hashPrefix('!');

}]);


foodtruckFrontendApp.service('foodTruckLocationService', function($http) {

    var getLocations = function(days, target){
        $http.get(apiUrl+'truck/getAllTruckLocations/'+days).success(function(data) {
            //console.log("PublicFoodTruckRoadMap : " + data.truckLocations);
            target(data.truckLocations);
        });
    };

    return {
        getTruckLocations: getLocations
    };

});


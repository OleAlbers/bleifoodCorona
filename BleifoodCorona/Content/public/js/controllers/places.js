/**
 * Created by Tomato on 23.06.2015.
 */


foodtruckApp.controller('PlacesController', function ($scope, $http, $location, $anchorScroll, $modal) {

    $scope.niceMonth=function(date) {
        monthNames=["Jan","Feb","Mär","Apr","Mai","Jun","Jul","Aug","Sep","Okt","Nov","Dez"];
        var month=date.getMonth();
        return monthNames[month];
    },
    $scope.places = [],
        $scope.getPlaces=function() {
            var heute=new Date();
            heute.setHours(0,0,0,0);
            $http.get(apiUrl + "company/get/mine")
                .success(function(data,status,headers,config){
                    data.Trucks.forEach(function(truck) {
                       truck.Locations.forEach(function(location) {
                           var truckLocation=location;
                           truckLocation.start=new Date(truckLocation.startDate);
                           truckLocation.end = new Date(location.endDate.substr(0, 4), location.endDate.substr(5, 2)-1, location.endDate.substr(8, 2), location.endDate.substr(11, 2), location.endDate.substr(14, 2), location.endDate.substr(17, 2));
                           if (truckLocation.end>=heute) {
                               truckLocation.truckName = truck.name;

                               truckLocation.truckId = truck.id;
                               $scope.places.push(truckLocation);
                           }
                        });
                    });

                 //   $scope.places=data.Locations;
                })
                .error(function (data, status, headers, config) {
                    redirectOnError(status);
                });
        },
        $scope.init=function() {
            $scope.getPlaces();
            console.log("Init Locations");
        }, $scope.init();
});
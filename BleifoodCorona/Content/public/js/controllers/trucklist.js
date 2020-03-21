

foodtruckFrontendApp.controller('PublicTrucks', function ($scope, $http) {

    $scope.currentCity="Deutschland";

    $scope.GetNetworkIcon=function(name) {
        switch (name) {
            case "Twitter":
                return "fa-twitter";
                break;
            case "Facebook":
                return "fa-facebook-square";
                break;
            case "Google+":
                return "fa-google-plus-square";
                break;
            case "Impressum":
                return "fa-info-circle";
                break;
            case "Website":
                return "fa-globe";
                break;
        }
    };

    $scope.getTrucksFor=function($city) {
        if ($city == 'Deutschland') {
            $city = 'any';
        }
        $http.get(apiUrl + 'truck/getTrucksIn/' + $city).success(function (data) {
            //console.log("PublicFoodTruckRoadMap : " + data.truckLocations);

            $scope.trucks = data;

            $scope.trucks.forEach(function(truck) {
                $http.get(apiUrl + 'company/get/' + truck.companyId+'/false').success(function (companyData) {
                    truck.company=companyData;
                    truck.company.Contacts.forEach(function (contact) {
                        if (contact.mode=='Public') {
                            truck.company.public=contact;
                        }
                    });
                });
            });
        });
    };

    $scope.getTrucks=function() {
        $scope.getTrucksFor($scope.currentCity);
    };
    $scope.getCities=function() {
        $scope.cities=[];
        $scope.cities.push("Deutschland");
        $http.get(apiUrl + 'truck/getCities').success(function (data) {
            data.forEach(function(city){
                $scope.cities.push(city.city);
            });
        });
    };
    $scope.init= function () {
        $scope.getCities();
        $scope.getTrucks();
        $startCity=search("id");
        if ($startCity!=null && $startCity!==undefined) {
            $scope.getTrucksFor($startCity);
        }
        /*angular.element(document).ready(function () {
            $scope.loadMaps();
        });*/
    };

    $scope.mapOptions= {
        zoom: 14,
        mapTypeId: google.maps.MapTypeId.TERRAIN,
        // zoomControl: false,
        mapTypeControl: false,
        rotateControl: false,
        scaleControl: false,
        streetViewControl:false
    };

    $scope.initMap=function(truck) {
        $scope.truckId  =truck.truckId;
        $scope.map=document.getElementById("smallMap"+truck.truckId);
        $scope.markers=[];
        var mapOptions=$scope.mapOptions;
        var myLatLng = new google.maps.LatLng(truck.latitude, truck.longitude);
        mapOptions.center=myLatLng;
        var singlemap = new google.maps.Map($scope.map, mapOptions);


        var marker = new google.maps.Marker({
            position: myLatLng,
            map: singlemap,
            icon: 'html/assets/images/marker-icon.png',
            title: truck.locationName,
            zIndex: parseInt(truck.locationId),
            animation: 2,
            truckId: parseInt(truck.truckId)
        });



    }
});


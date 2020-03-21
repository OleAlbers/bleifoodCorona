/*
function codeAddress() {
    //var address = document.getElementById("address").value;
    var address = "sievekingsalle 188b, 22111 Hamburg";
    geocoder.geocode( { 'address': address}, function(results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);
            var marker = new google.maps.Marker({
                map: map,
                position: results[0].geometry.location
            });
        } else {
            alert("Geocode was not successful for the following reason: " + status);
        }
    });
}
*/

foodtruckFrontendApp.controller('PublicFoodTruckRoadMap', function ($scope, $http, $modal, foodTruckLocationService) {

    $scope.currentDateWord='Heute';
    $scope.currentDateSelected=0;
    $scope.fertich=function(data) {
        $scope.trucks = data;
    };

    $scope.init = function (){
        //$scope.refreshTruckLocationData($scope.currentDateSelected);
        $scope.trucks = foodTruckLocationService.getTruckLocations($scope.currentDateSelected, $scope.fertich);
        $startDay=search("id");
        if ($startDay!=null && $startDay!==undefined) {
            switch  ($startDay) {
                case "morgen":
                    $scope.currentDateSelected=1;
                    $scope.refreshTruckLocationData(1);
                    break;
                case "uebermorgen":
                    $scope.refreshTruckLocationData(2);
                    $scope.currentDateSelected=2;
                    break;
            }
            $scope.setDateWord();
        }
    };
    $scope.refreshTruckLocationData = function (days) {
        $http.get(apiUrl+'truck/getAllTruckLocations/'+days).success(function(data) {
            $scope.trucks = data.truckLocations;
            console.log("PublicFoodTruckRoadMap : " + $scope.trucks);
        });
    },
    $scope.moveDate = function (pointer) {

        if(pointer=='-'){
            if($scope.currentDateSelected!=0){
                $scope.currentDateSelected=--$scope.currentDateSelected;
            }
        }else{
            if($scope.currentDateSelected<8) {
                $scope.currentDateSelected = ++$scope.currentDateSelected;
            }
        }
        //console.log($scope.currentDateSelected);
        $scope.setDateWord();
        $scope.refreshTruckLocationData($scope.currentDateSelected);
    },
    $scope.setDateWord = function (){

        var d = new Date();

        switch($scope.currentDateSelected) {
            case 0:
                $scope.currentDateWord='Heute';
                break;
            case 1:
                $scope.currentDateWord='Morgen';
                break;
            case 2:
                $scope.currentDateWord='Übermorgen';
                break;
            case 3:
                d.setDate(d.getDate() + $scope.currentDateSelected);
                $scope.currentDateWord=''+d.getDate()+'.'+ (d.getMonth()+1)+'.'+ d.getFullYear();
                break;
            case 4:
                d.setDate(d.getDate() + $scope.currentDateSelected);
                $scope.currentDateWord=''+d.getDate()+'.'+ (d.getMonth()+1)+'.'+ d.getFullYear();
                break;
            case 5:
                d.setDate(d.getDate() + $scope.currentDateSelected);
                $scope.currentDateWord=''+d.getDate()+'.'+ (d.getMonth()+1)+'.'+ d.getFullYear();
                break;
            case 6:
                d.setDate(d.getDate() + $scope.currentDateSelected);
                $scope.currentDateWord=''+d.getDate()+'.'+ (d.getMonth()+1)+'.'+ d.getFullYear();
                break;
            case 7:
                d.setDate(d.getDate() + $scope.currentDateSelected);
                $scope.currentDateWord=''+d.getDate()+'.'+ (d.getMonth()+1)+'.'+ d.getFullYear();
                break;
            default:
                $scope.currentDateWord='Unbekannt';
        }
    },
    $scope.init();


});

foodtruckFrontendApp.controller('UserPageController6', function ($scope) {
    //$scope.Area = { Name: "Melbourne", Latitude: -37.8140000, Longitude: 144.9633200 };
});

foodtruckFrontendApp.controller('UserPageController5', function ($scope) {
    $scope.Area = { Name: "Melbourne", Latitude: -37.8140000, Longitude: 144.9633200 };
    $scope.Adress = "22179 Hamburg, Fahrenkrön 85, Deutschland";
    var address = "22179 Hamburg, Fahrenkrön 85, Deutschland";



    getAddr(address, function(status, res) {
        // blah blah, whatever you would do with
        // what was returned from getAddr previously
        // you just use res instead
        // For example:
        if (status == 'ok') {

            //$scope.Area = { Name: "Ole", Latitude: res.latitude, Longitude: res.longitude };
            alert(res.latitude + " " +res.longitude);

        } else {
            alert("Error")
        }
    });


});


foodtruckFrontendApp.controller('InfoController', function ($scope, $log) {
    $scope.templateValue = 'hello from the template itself';
    $scope.clickedButtonInWindow = function () {
        var msg = 'clicked a window in the template!';
        $log.info(msg);
        alert(msg);
    }
});

/**
 * Viele Marker auf der Map anzeigen und an die äußersten Marker das Bild anpassen (bounds)
 */
foodtruckFrontendApp.controller('UserPageController3', function($scope) {
    $scope.map = {

        center: {
            latitude: 40.1451,
            longitude: -99.6680
        },
        zoom: 4,
        bounds: {}
    };
    $scope.options = {
        scrollwheel: false
    };
    var createRandomMarker = function(i, bounds, idKey) {
        var lat_min = bounds.southwest.latitude,
            lat_range = bounds.northeast.latitude - lat_min,
            lng_min = bounds.southwest.longitude,
            lng_range = bounds.northeast.longitude - lng_min;

        if (idKey == null) {
            idKey = "id";
        }

        var latitude = lat_min + (Math.random() * lat_range);
        var longitude = lng_min + (Math.random() * lng_range);
        var ret = {
            latitude: latitude,
            longitude: longitude,
            title: 'm' + i
        };
        ret[idKey] = i;
        return ret;
    };
    $scope.randomMarkers = [];
    // Get the bounds from the map once it's loaded
    $scope.$watch(function() {
        return $scope.map.bounds;
    }, function(nv, ov) {
        // Only need to regenerate once
        if (!ov.southwest && nv.southwest) {
            var markers = [];
            for (var i = 0; i < 50; i++) {
                markers.push(createRandomMarker(i, $scope.map.bounds))
            }
            $scope.randomMarkers = markers;
        }
    }, true);
});

foodtruckFrontendApp.config(function(uiGmapGoogleMapApiProvider) {
    uiGmapGoogleMapApiProvider.configure({
        //    key: 'your api key',
        v: '3.17',
        libraries: 'weather,geometry,visualization'
    });
});

/**
 * Marker Drag&Drop mit Anzeige der lon/lat
 * Marker automatisch bewegen lassen
 */
foodtruckFrontendApp.controller('UserPageController2', function ($scope, $log, $timeout) {

    $scope.map = {center: {latitude: 51.219053, longitude: 10.6161677 }, zoom: 6 };
    $scope.options = {scrollwheel: false};
    $scope.coordsUpdates = 0;
    $scope.dynamicMoveCtr = 0;
    $scope.marker = {
        id: 0,
        coords: {
            latitude: 51.219053,
            longitude: 10.6161677
        },
        options: {
            draggable: true,
            //icon: 'https://maps.google.com/mapfiles/ms/icons/green-dot.png'
            icon: 'html/assets/images/marker-icon.png'
        },
        events: {
            dragend: function (marker, eventName, args) {
                $log.log('marker dragend');
                var lat = marker.getPosition().lat();
                var lon = marker.getPosition().lng();
                $log.log(lat);
                $log.log(lon);

                $scope.marker.options = {
                    draggable: true,
                    icon: 'html/assets/images/marker-icon.png',
                    labelContent: "lat: " + $scope.marker.coords.latitude + ' ' + 'lon: ' + $scope.marker.coords.longitude,
                    labelAnchor: "100 0",
                    labelClass: "marker-labels"
                };
            }
        }
    };
    $scope.$watchCollection("marker.coords", function (newVal, oldVal) {
        if (_.isEqual(newVal, oldVal))
            return;
        $scope.coordsUpdates++;
    });
    $timeout(function () {
        $scope.marker.coords = {
            latitude: 50.219053,
            longitude: 10.6161677
        };
        $scope.dynamicMoveCtr++;
        $timeout(function () {
            $scope.marker.coords = {
                latitude: 49.219053,
                longitude: 10.6161677
            };
            $scope.dynamicMoveCtr++;
        }, 2000);
    }, 1000);
});


/**
 * Hiermit kann man eine Adresse eingeben und diese per Klick anzeigen lassen,
 * ebenfalls kann der aktuelle Standort ermittelt werden.
 */
foodtruckFrontendApp.controller('UserPageController1', function($scope) {


    // current location
    $scope.loc = { lat: 40, lon: -73 };
    $scope.gotoCurrentLocation = function () {
        if ("geolocation" in navigator) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var c = position.coords;
                $scope.gotoLocation(c.latitude, c.longitude);
            });
            return true;
        }
        return false;
    };
    $scope.gotoLocation = function (lat, lon) {
        if ($scope.lat != lat || $scope.lon != lon) {
            $scope.loc = { lat: lat, lon: lon };
            if (!$scope.$$phase) $scope.$apply("loc");
        }
    };

    // geo-coding
    $scope.search = "";
    $scope.geoCode = function () {
        if ($scope.search && $scope.search.length > 0) {
            if (!this.geocoder) this.geocoder = new google.maps.Geocoder();
            this.geocoder.geocode({ 'address': $scope.search }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    var loc = results[0].geometry.location;
                    $scope.search = results[0].formatted_address;
                    $scope.gotoLocation(loc.lat(), loc.lng());
                } else {
                    alert("Sorry, this search produced no results.");
                }
            });
        }
    };


});

// formats a number as a latitude (e.g. 40.46... => "40°27'44"N")
foodtruckFrontendApp.filter('lat', function () {
    return function (input, decimals) {
        if (!decimals) decimals = 0;
        input = input * 1;
        var ns = input > 0 ? "N" : "S";
        input = Math.abs(input);
        var deg = Math.floor(input);
        var min = Math.floor((input - deg) * 60);
        var sec = ((input - deg - min / 60) * 3600).toFixed(decimals);
        return deg + "°" + min + "'" + sec + '"' + ns;
    }
});

// formats a number as a longitude (e.g. -80.02... => "80°1'24"W")
foodtruckFrontendApp.filter('lon', function () {
    return function (input, decimals) {
        if (!decimals) decimals = 0;
        input = input * 1;
        var ew = input > 0 ? "E" : "W";
        input = Math.abs(input);
        var deg = Math.floor(input);
        var min = Math.floor((input - deg) * 60);
        var sec = ((input - deg - min / 60) * 3600).toFixed(decimals);
        return deg + "°" + min + "'" + sec + '"' + ew;
    }
});

// - Documentation: https://developers.google.com/maps/documentation/
foodtruckFrontendApp.directive("appMap", function () {
    return {
        restrict: "E",
        replace: true,
        template: "<div></div>",
        scope: {
            center: "=",        // Center point on the map (e.g. <code>{ latitude: 10, longitude: 10 }</code>).
            markers: "=",       // Array of map markers (e.g. <code>[{ lat: 10, lon: 10, name: "hello" }]</code>).
            width: "@",         // Map width in pixels.
            height: "@",        // Map height in pixels.
            zoom: "@",          // Zoom level (one is totally zoomed out, 25 is very much zoomed in).
            mapTypeId: "@",     // Type of tile to show on the map (roadmap, satellite, hybrid, terrain).
            panControl: "@",    // Whether to show a pan control on the map.
            zoomControl: "@",   // Whether to show a zoom control on the map.
            scaleControl: "@"   // Whether to show scale control on the map.
        },
        link: function (scope, element, attrs) {
            var toResize, toCenter;
            var map;
            var currentMarkers;

            // listen to changes in scope variables and update the control
            var arr = ["width", "height", "markers", "mapTypeId", "panControl", "zoomControl", "scaleControl"];
            for (var i = 0, cnt = arr.length; i < arr.length; i++) {
                scope.$watch(arr[i], function () {
                    cnt--;
                    if (cnt <= 0) {
                        updateControl();
                    }
                });
            }

            // update zoom and center without re-creating the map
            scope.$watch("zoom", function () {
                if (map && scope.zoom)
                    map.setZoom(scope.zoom * 1);
            });
            scope.$watch("center", function () {
                if (map && scope.center)
                    map.setCenter(getLocation(scope.center));
            });

            // update the control
            function updateControl() {

                // update size
                if (scope.width) element.width(scope.width);
                if (scope.height) element.height(scope.height);

                // get map options
                var options =
                {
                    center: new google.maps.LatLng(40, -73),
                    zoom: 6,
                    mapTypeId: "roadmap"
                };
                if (scope.center) options.center = getLocation(scope.center);
                if (scope.zoom) options.zoom = scope.zoom * 1;
                if (scope.mapTypeId) options.mapTypeId = scope.mapTypeId;
                if (scope.panControl) options.panControl = scope.panControl;
                if (scope.zoomControl) options.zoomControl = scope.zoomControl;
                if (scope.scaleControl) options.scaleControl = scope.scaleControl;

                // create the map
                map = new google.maps.Map(element[0], options);

                // update markers
                updateMarkers();

                // listen to changes in the center property and update the scope
                google.maps.event.addListener(map, 'center_changed', function () {

                    // do not update while the user pans or zooms
                    if (toCenter) clearTimeout(toCenter);
                    toCenter = setTimeout(function () {
                        if (scope.center) {

                            // check if the center has really changed
                            if (map.center.lat() != scope.center.lat ||
                                map.center.lng() != scope.center.lon) {

                                // update the scope and apply the change
                                scope.center = { lat: map.center.lat(), lon: map.center.lng() };
                                if (!scope.$$phase) scope.$apply("center");
                            }
                        }
                    }, 500);
                });
            }

            // update map markers to match scope marker collection
            function updateMarkers() {
                if (map && scope.markers) {

                    // clear old markers
                    if (currentMarkers != null) {
                        for (var i = 0; i < currentMarkers.length; i++) {
                            currentMarkers[i] = m.setMap(null);
                        }
                    }

                    // create new markers
                    currentMarkers = [];
                    var markers = scope.markers;
                    if (angular.isString(markers)) markers = scope.$eval(scope.markers);
                    for (var i = 0; i < markers.length; i++) {
                        var m = markers[i];
                        var loc = new google.maps.LatLng(m.lat, m.lon);
                        var mm = new google.maps.Marker({ position: loc, map: map, title: m.name });
                        currentMarkers.push(mm);
                    }
                }
            }

            // convert current location to Google maps location
            function getLocation(loc) {
                if (loc == null) return new google.maps.LatLng(40, -73);
                if (angular.isString(loc)) loc = scope.$eval(loc);
                return new google.maps.LatLng(loc.lat, loc.lon);
            }
        }
    };
});

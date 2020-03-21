foodtruckFrontendApp.directive('addressBasedGoogleMap', function () {
    return {
        restrict: "A",
        template: "<div id='addressMap'></div>",
        scope: {
            address: "=",
            zoom: "="
        },
        controller: function ($scope) {
            var geocoder;
            var latlng;
            var map;
            var marker;
            var initialize = function () {
                geocoder = new google.maps.Geocoder();
                latlng = new google.maps.LatLng(-34.397, 150.644);
                var mapOptions = {
                    zoom: $scope.zoom,
                    center: latlng,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };

                map = new google.maps.Map(document.getElementById('addressMap'), mapOptions);

                google.maps.event.addListener(map, 'click', function(event) {
                    //placeMarker(event.latLng);
                    alert("MAP");
                    console.log("MAP");
                });



            };
            var markAdressToMap = function () {
                geocoder.geocode({ 'address': $scope.address }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        map.setCenter(results[0].geometry.location);
                        marker = new google.maps.Marker({
                            map: map,
                            position: results[0].geometry.location,
                            animation: 2,
                            icon: 'html/assets/images/marker-icon.png',
                            title: 'Bleifood.de'
                        });

                        google.maps.event.addListener(marker, 'click', function(event) {
                            //placeMarker(event.latLng);
                            alert("MARKER");
                            console.log("MARKER");
                        });
                    }
                });
            };



            $scope.$watch("address", function () {

                if ($scope.address != undefined) {
                    markAdressToMap();
                }
            });
            google.maps.event.addDomListener(window, 'load', initialize());
            //initialize();


        }
    };
});



foodtruckFrontendApp.directive('areaBasedGoogleMap23', function () {
    return {
        restrict: "A",
        template: "<div id='areaMap'></div>",
        scope: {           
            area: "=",
            zoom: "="
        },
        controller: function ($scope) {
            var mapOptions;
            var map;           
            var marker;

            var initialize = function () {                                
                mapOptions = {
                    zoom: $scope.zoom,
                    //center: new google.maps.LatLng(40.0000, -98.0000),
                    center: new google.maps.LatLng(-34.397, 150.644),
                    mapTypeId: google.maps.MapTypeId.TERRAIN
                };
                map = new google.maps.Map(document.getElementById('areaMap'), mapOptions);
            };
          
            var createMarker = function (area) {
                var position = new google.maps.LatLng(area.Latitude, area.Longitude);
                map.setCenter(position);
                marker = new google.maps.Marker({
                    map: map,
                    position: position,
                    title: area.Name
                });               
            };

            $scope.$watch("area", function (area) {
                if (area != undefined) {
                    createMarker(area);
                }
            });

            initialize();
        }
    };
});

foodtruckFrontendApp.directive('showAllFoodtrucksMapOk', function () {
    return {
        restrict: "A",
        template: "<div id='allFoodTruckOk'></div>",
        scope: {
            area: "=",
            zoom: "="
        },
        controller: function ($scope) {
            var mapOptions;
            var map;
            var marker;

            var initialize = function () {
                mapOptions = {
                    zoom: 12,
                    center: new google.maps.LatLng(53.562603271499746, 9.985312139697271),
                    mapTypeId: google.maps.MapTypeId.TERRAIN
                };
                map = new google.maps.Map(document.getElementById('allFoodTruckOk'), mapOptions);

                google.maps.event.addListener(map, 'click', function(event) {
                    //placeMarker(event.latLng);
                    alert("MAP");
                    console.log("MAP");
                });

                //setMarkers(map, beaches);
                setMarkers(map, foodtrucks);
            };



            var foodtrucks = [
                ['Mega Burger', 53.55422920243254, 9.968768274493414, 4, 'html/assets/images/foodtruck-marker.png'],
                ['Crazy Croque', 53.54959853560326, 9.965265309519964, 5, 'html/assets/images/foodtruck-marker5.png'],
                ['Lecker Bleifood', 53.583897419043716, 10.027905618853765, 3,'html/assets/images/foodtruck-marker2.png'],
                ['FatnFunny Pizza', 53.595525833410605, 10.03101698131104, 2,'html/assets/images/foodtruck-marker3.png'],
                ['Oles Frittentheke', 53.562603271499746, 9.985312139697271, 6,'html/assets/images/marker-icon.png'],
                ['Die Macht im Norden', 53.58706593780408, 9.899395620532232, 6,'html/assets/images/marker-icon.png'],
                ['Wurst Bude', 53.614343581718074, 10.097256815142828, 1,'html/assets/images/foodtruck-marker4.png']
            ];


            var setMarkers = function (map, locations) {

                // Add markers to the map

                // Marker sizes are expressed as a Size of X,Y
                // where the origin of the image (0,0) is located
                // in the top left of the image.

                // Origins, anchor positions and coordinates of the marker
                // increase in the X direction to the right and in
                // the Y direction down.
                var image = {
                    url: 'html/assets/images/foodtruck-marker.png',
                    // This marker is 20 pixels wide by 32 pixels tall.
                    size: new google.maps.Size(20, 32),
                    // The origin for this image is 0,0.
                    origin: new google.maps.Point(0,0),
                    // The anchor for this image is the base of the flagpole at 0,32.
                    anchor: new google.maps.Point(0, 32)
                };
                // Shapes define the clickable region of the icon.
                // The type defines an HTML &lt;area&gt; element 'poly' which
                // traces out a polygon as a series of X,Y points. The final
                // coordinate closes the poly by connecting to the first
                // coordinate.
                var shape = {
                    coords: [1, 1, 1, 20, 18, 20, 18 , 1],
                    type: 'poly'
                };

                for (var i = 0; i < locations.length; i++) {
                    var truckLocation = locations[i];
                    var myLatLng = new google.maps.LatLng(truckLocation[1], truckLocation[2]);
                    var marker = new google.maps.Marker({
                        position: myLatLng,
                        map: map,
                        //icon: image,
                        //icon: beach[4],
                        icon: 'html/assets/images/marker-icon.png',
                        //shape: shape,
                        title: truckLocation[0],
                        zIndex: truckLocation[3],
                        animation: 2
                    });
                    google.maps.event.addListener(marker, 'click', function(event) {
                        
                        showPopup("delete", "Foodtruck", "keine Ahnung grade", "YesNo",  "", "", "");
                        console.log("MARKER");
                    });

                }


            };

            $scope.$watch("area", function (area) {
                if (area != undefined) {
                }
            });

            initialize();
        }
    };
});

foodtruckFrontendApp.directive('showAllFoodtrucksMap', function ($http, $location, $anchorScroll) {
    //var mapOptions;
    var map;
    var marker;
    var foodtrucks;
    var mapOptions = {
        zoom: 10,
        center: new google.maps.LatLng(0, 0),
        mapTypeId: google.maps.MapTypeId.TERRAIN
    };
    var currentDateSelected=0;
    var markersArray = [];

    function clearOverlays() {
        for (var i = 0; i < markersArray.length; i++ ) {
            markersArray[i].setMap(null);
        }
        markersArray.length = 0;
    }

    /* automatisches anpassen der Karte an verwendete marker positionen */
    var bounds = new google.maps.LatLngBounds();

    var setTrucks = function (day){
        //map = new google.maps.Map(document.getElementById('allFoodTruck'), mapOptions);

        $http.get(apiUrl+'truck/getAllTruckLocations/'+day).success(function(data) {
            foodtrucks = data.truckLocations;
            setMarkers(map, foodtrucks);
        });
    }

    var showModalTruck = function(id) {
        $location.hash('truck'+id);
        $anchorScroll();
/*        var modalInstance = $modal.open({
            animation: true,
            templateUrl: 'popups/truck.html',
            controller: 'TruckControllerModal',
            windowClass: 'truckWin',
            backdropClass:'truckBackdrop',
            size: 'lg',
            resolve : {
                id: function() { return id; }
            }
        });*/
        return false;
    }

    function clickToOpenModal(marker, truckId) {

        marker.addListener('click', function() {
            showModalTruck(truckId);
        });
    }

    var setMarkers = function (map, locations) {

        for (var i = 0; i < locations.length; i++) {
            var truckLocation = locations[i];
            var myLatLng = new google.maps.LatLng(truckLocation.latitude, truckLocation.longitude);
            var marker = new google.maps.Marker({
                position: myLatLng,
                map: map,
                icon: 'html/assets/images/marker-icon.png',
                title: truckLocation.name,
                animation: 2,
                truckId: parseInt(truckLocation.truckId)
            });

            clickToOpenModal(marker, truckLocation.truckId);
/*
            marker.addListener('click', function() {
                map.setZoom(8);
                map.setCenter(marker.getPosition());
                console.log("MARKER title: " + marker.title);
                console.log("MARKER position: " + marker.position);
            });
*/
            //console.log("truckLocation.id: " + truckLocation.truckId);
/*
            google.maps.event.addListener(marker, 'click', function(event) {

                //showPopup("delete", "Foodtruck", "keine Ahnung grade", "YesNo",  "", "", "");

                //showModalTruck(marker.truckId);

                console.log("MARKER title: " + marker.title);
                console.log("MARKER position: " + marker.position);
                //console.log("MARKER zIndex: " + marker.zIndex);
                console.log("MARKER truckId: " + marker.truckId);

            });
*/
            /* automatisches anpassen der Karte an verwendete marker positionen */
            bounds.extend(myLatLng);
            markersArray.push(marker);
        }
        /* automatisches anpassen der Karte an verwendete marker positionen */
        map.fitBounds(bounds);
    }

    return {
        restrict: "A",
        template: "<div id='allFoodTruck'></div>",
        scope: {
            area: "=",
            zoom: "="
        },
        link: function (scope, element, attrs) {

            var onButtonClickNext = function () {

                if(currentDateSelected<8) {
                    currentDateSelected = ++currentDateSelected;
                    clearOverlays();
                    setTrucks(currentDateSelected);
                }
            };

            var onButtonClickPrev = function () {

                if(currentDateSelected!=0) {
                    currentDateSelected = --currentDateSelected;
                    clearOverlays();

                    setTrucks(currentDateSelected);
                }
            };

            var buttonNext = angular.element(document.querySelector('#NextDateTrucks'));
            var buttonPrev = angular.element(document.querySelector('#PrevDateTrucks'));

            buttonNext.on('click', onButtonClickNext);
            scope.$on('$destroy', function () {
                buttonNext.off('click', onButtonClickNext);
            });

            buttonPrev.on('click', onButtonClickPrev);
            scope.$on('$destroy', function () {
                buttonPrev.off('click', onButtonClickPrev);
            });
        },
        controller: function ($scope, $http) {
            /*var mapOptions;
            var map;
            var marker;
            var foodtrucks;*/

            var initialize = function () {


                map = new google.maps.Map(document.getElementById('allFoodTruck'), mapOptions);

                setTrucks(0);

            };

            // automatisches anpassen der Karte an verwendete marker positionen
/*            var bounds = new google.maps.LatLngBounds();

            var setMarkers = function (map, locations) {

                for (var i = 0; i < locations.length; i++) {
                    var truckLocation = locations[i];
                    var myLatLng = new google.maps.LatLng(truckLocation.latitude, truckLocation.longitude);
                    var marker = new google.maps.Marker({
                        position: myLatLng,
                        map: map,
                        icon: 'html/assets/images/marker-icon.png',
                        title: truckLocation.name,
                        zIndex: parseInt(truckLocation.locationId),
                        animation: 2
                    });
                    google.maps.event.addListener(marker, 'click', function(event) {

                        showPopup("delete", "Foodtruck", "keine Ahnung grade", "YesNo",  "", "", "");
                        console.log("MARKER");
                    });
                    // automatisches anpassen der Karte an verwendete marker positionen
                    bounds.extend(myLatLng);
                }
                // automatisches anpassen der Karte an verwendete marker positionen
                map.fitBounds(bounds);
            }
*/

            initialize();
        }
    };
});

foodtruckFrontendApp.directive('showSelectedFoodtrucksMap', function ($http) {
    var map;
    var marker;
    var foodtrucks;
    var mapOptions = {
        zoom: 10,
        center: new google.maps.LatLng(0, 0),
        mapTypeId: google.maps.MapTypeId.TERRAIN
    };
    var currentDateSelected=0;
    var markersArray = [];

    function clearOverlays() {
        for (var i = 0; i < markersArray.length; i++ ) {
            markersArray[i].setMap(null);
        }
        markersArray.length = 0;
    }
    var globalTruckId = 0;

    /* automatisches anpassen der Karte an verwendete marker positionen */
    var bounds = new google.maps.LatLngBounds();

    var setTrucks = function (truckId, day){
        $http.get(apiUrl+'truck/getTruckLocation/'+truckId+'/'+day).success(function(data) {
            foodtrucks = data.truckLocations;
            setMarkers(map, foodtrucks);
        });
    }

    var setMarkers = function (map, locations) {

        for (var i = 0; i < locations.length; i++) {
            var truckLocation = locations[i];
            var myLatLng = new google.maps.LatLng(truckLocation.latitude, truckLocation.longitude);
            var marker = new google.maps.Marker({
                position: myLatLng,
                map: map,
                icon: 'html/assets/images/marker-icon.png',
                title: truckLocation.name,
                zIndex: parseInt(truckLocation.locationId),
                animation: 2,
                truckId: parseInt(truckLocation.truckId)
            });

            //console.log("truckLocation.id: " + truckLocation.truckId);

            google.maps.event.addListener(marker, 'click', function(event) {

                /*
                 console.log("MARKER title: " + marker.title);
                 console.log("MARKER position: " + marker.position);
                 console.log("MARKER zIndex: " + marker.zIndex);
                 console.log("MARKER truckId: " + marker.truckId);
                 */
            });
            /* automatisches anpassen der Karte an verwendete marker positionen */
            bounds.extend(myLatLng);
            markersArray.push(marker);
        }
        /* automatisches anpassen der Karte an verwendete marker positionen */
        map.fitBounds(bounds);
    }

    return {
        restrict: "A",
        template: "<div id='selectedFoodTruck'></div>",
        scope: {
            area: "=",
            zoom: "="
        },
        link: function (scope, element, attrs) {

            //globalTruckId = truckService;

            var onButtonClickNext = function () {

                if(currentDateSelected<8) {
                    currentDateSelected = ++currentDateSelected;
                    clearOverlays();
                    setTrucks(globalTruckId,currentDateSelected);
                }
            };

            var onButtonClickPrev = function () {

                if(currentDateSelected!=0) {
                    currentDateSelected = --currentDateSelected;
                    clearOverlays();
                    setTrucks(globalTruckId,currentDateSelected);
                }
            };

            var buttonNext = angular.element(document.querySelector('#NextDateTrucks'));
            var buttonPrev = angular.element(document.querySelector('#PrevDateTrucks'));

            buttonNext.on('click', onButtonClickNext);
            scope.$on('$destroy', function () {
                buttonNext.off('click', onButtonClickNext);
            });

            buttonPrev.on('click', onButtonClickPrev);
            scope.$on('$destroy', function () {
                buttonPrev.off('click', onButtonClickPrev);
            });
        },
        controller: function ($scope, $http) {

            var initialize = function () {

                //globalTruckId = document.getElementById('selectedTruckId').value;
                map = new google.maps.Map(document.getElementById('selectedFoodTruck'), mapOptions);
                setTrucks(globalTruckId,0);
            };
            initialize();
        }
    };
});
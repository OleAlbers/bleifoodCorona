foodtruckApp.controller('TrucksController', function ($scope, $http, $location, $anchorScroll, $modal) {
    $scope.trucks = [],
        $scope.dateFormat = 'dd.MM.yyyy',
        $scope.minDate = '2015-01-01',
        $scope.maxDate = '2017-12-31',
        $scope.timeRegex = '^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$',
        $scope.truckNotSaved = false,
        $scope.noPicture = 'public/img/no_photo.png';

        $scope.init = function () {
            console.log("init");
            $scope.refreshTruckData();
        };
        $scope.truckDetail = {
            Show: false, ShowEdit: false
        };
        $scope.closeTruckDetail = function () {
            $scope.truckDetail = {
                Show: false, ShowEdit: false
            };
            $scope.refreshTruckData();
        };
        $scope.createNewTruck = function () {
            $scope.closeAllEdits();
            $scope.truckDetail = {
                Show: true, ShowEdit: false, id: null, AssignedCards: [], locations: []
            };
            $scope.truckDetail.unassignedCards=$scope.getNonassignedCards($scope.truckDetail);
            $scope.initNewLocation();
        };
        $scope.refreshTruckData = function () {
            console.log("refreshTruckData");
            $http.get(apiUrl + 'company/getAllTrucks/mine/true')
                .success(function (data, status, headers, config) {
                    $scope.trucks = data;
                    $scope.trucks.forEach(function (truck) {
                        truck.ShowEdit = false;
                        truck.Saved = true;
                        truck.Published = truck.IsPublished == "1";
                        if (truck.AssignedCards!==undefined) {
                            if (truck.AssignedCards.length==1) {
                                truck.card=truck.AssignedCards[0].name;
                            }
                            if (truck.AssignedCards.length>1) {
                                truck.card=truck.AssignedCards[0]+" (und "+(truck.AssignedCards.length-1)+' weitere)';
                            }
                        }
                        if (truck.Today!==undefined && truck.Today.name!==undefined) {
                            truck.city=truck.Today.city;
                            truck.zip=truck.Today.zip;
                            truck.street=truck.Today.street;
                        }

                        truck.imageCheck=truck.TruckImages.length;
                        truck.images=truck.TruckImages;

                        for(var i = 0; i<3; i++){
                            if (truck.images[i]!=null){
                                truck.images[i].path="public/img/"+truck.images[i].companyId+"/truck/"+truck.images[i].imageId+"_150x150.jpg";
                            } else {
                                var newTruck = [];
                                newTruck.path="public/img/no_photo.png";
                                truck.images.push(newTruck);
                            }
                        }

                    });
                })
                .error(function (data, status, headers, config) {
                    if (status == 404) {
                        // Noch keine Trucks angelegt; Macht nix :)
                        readAddresses();
                    } else {
                        redirectOnError(status);
                    }
                });
            // Alle Karten:
            $http.get(apiUrl + 'card/getAll/mine/false')
                .success(function (data, status, headers, config) {
                    $scope.cards = data;
                });
            // Alle Locations:
            $http.get(apiUrl + 'location/getAll/mine')
                .success(function (data, status, headers, config) {
                    $scope.oldLocations = data;
                    $scope.oldLocationCopied = {};
                });
        };
        $scope.closeAllEdits = function () {
            $scope.trucks.forEach(function (truck) {
                truck.ShowEdit = false;
            });
        };

        $scope.doDrop=function(newlocation) {
            if ($scope.truckDetail.ShowEdit || $scope.truckDetail.Show)  {
                $scope.dropScope.city=newlocation.city;
                $scope.dropScope.zip=newlocation.zip;
                $scope.dropScope.street=newlocation.street;

            }
        };
        $scope.setDropScope=function(scope) {
          $scope.dropScope=scope;
        };

        $scope.generateTimes = function (start, end) {
            var times = [];
            for (i = start; i <= end; i++) {
                var prefix = (i < 10 ? "0" : "");
                prefix += i + ":";
                times.push(prefix + "00");
                if (i != end) {
                    times.push(prefix + "15");
                    times.push(prefix + "30");
                    times.push(prefix + "45");
                }
            }
            return times;
        };
        $scope.editTruck = function (truck, $event) {
            $scope.closeAllEdits();
            $scope.truckDetail = truck;
            $scope.truckDetail.ShowEdit = true;
            $scope.truckDetail.unassignedCards = $scope.getNonassignedCards(truck);
            $scope.initNewLocation();
            $scope.truckDetail.locations = [];
            // Aktuelle Termine laden:
            if (truck.id !== undefined && truck.id !== null) {
                $http.get(apiUrl + 'truck/getLocations/' + truck.id + '/true')
                    .success(function (data, status, headers, config) {
                        $scope.truckDetail.locations = data;

                        $scope.truckDetail.locations.forEach(function (location) {

                            location.startDate = new Date(location.startDate.substr(0, 4), location.startDate.substr(5, 2)-1, location.startDate.substr(8, 2), location.startDate.substr(11, 2), location.startDate.substr(14, 2), location.startDate.substr(17, 2));
                            location.endDate = new Date(location.endDate.substr(0, 4), location.endDate.substr(5, 2)-1, location.endDate.substr(8, 2), location.endDate.substr(11, 2), location.endDate.substr(14, 2), location.endDate.substr(17, 2));
                            location.Published=location.IsPublished==1;

                            location.startTime = location.startDate.getHours() > 9 ? "" : "0";
                            location.startTime += location.startDate.getHours() + ":";
                            location.startTime += location.startDate.getMinutes() > 9 ? "" : "0";
                            location.startTime += location.startDate.getMinutes();
                            location.endTime = location.endDate.getHours() > 9 ? "" : "0";
                            location.endTime += location.endDate.getHours() + ":";
                            location.endTime += location.endDate.getMinutes() > 9 ? "" : "0";
                            location.endTime += location.endDate.getMinutes();
                        });
                    })
                    .error(function (data, status, headers, config) {
                        if (status == 404) {
                            // Noch keine Locations angelegt; Macht nix :)
                        } else {
                            redirectOnError(status);
                        }
                    });
            }
        };
        $scope.addCardToTruck = function (card) {
            if (card === undefined) {
                return;
            }
            $scope.truckDetail.AssignedCards.push(card);
            $scope.truckDetail.unassignedCards = $scope.getNonassignedCards($scope.truckDetail);
        };
        $scope.removeCardFromTruck = function (card) {
            var index = $scope.truckDetail.AssignedCards.indexOf(card);
            $scope.truckDetail.AssignedCards.splice(index, 1);
            $scope.truckDetail.unassignedCards = $scope.getNonassignedCards($scope.truckDetail);
        };
    $scope.oldLocationSelected=function(location) {
        $scope.truckDetail.newLocation.city=location.city;
        $scope.truckDetail.newLocation.zip=location.zip;
        $scope.truckDetail.newLocation.street=location.street;
    };
        $scope.getNonassignedCards = function (truck) {
            if (truck.AssignedCards === undefined || $scope.cards === undefined) {
                return [];
            }
            var nonassigned = [];
            $scope.cards.forEach(function (card) {
                var assignedCard = $.grep(truck.AssignedCards, function (e) {
                    return e.name == card.name;
                });
                if (assignedCard.length == 0) {
                    nonassigned.push(card);
                }
            });
            return nonassigned;
        };
        $scope.saveTruck = function ($event) {
            clearAlert($($event.target).closest('.simpleForm'));
            $scope.truckDetail.CompanyId = "mine";

            // Benötigte Permissions: Employee
            $http.post(apiUrl + 'truck/set', {
                Truck: $scope.truckDetail
            }).
                success(function (data, status, headers, config) {
                    $scope.truckDetail.Id = data.Id;  // ID zuweisen. Falls INSERT werden danach die anderen DIVs sichtbar
                    //showAlert("success", "Speichern erfolgreich", undefined, $($event.target).closest('.easyBox'));
                    $scope.truckDetail = {};
                    $scope.truckDetail.AssignedCards = [];
                    $scope.refreshTruckData();
                }).
                error(function (data, status, headers, config) {
                    redirectOnError(status, $($event.target).closest('.simpleForm'));
                });
        };
        $scope.exportPdf=function(truck) {
            var modalInstance = $modal.open({
                animation: true,
                templateUrl: 'popups/exportModal.php?exportType=truck&id='+truck.id,
                controller: 'PdfController',
                windowClass: 'exportWin'
            });
        };
            $scope.youtube=function(helpId) {
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: 'popups/youtube.html',
                    controller: 'YtController',
                    windowClass: 'youtubeWin',
                    resolve : {
                        id: function() { return helpId; }
                    }
                });
            };

        $scope.deleteTruck = function (truck) {
            showPopup("delete", "Foodtruck löschen?", "Soll dieser wunderhübsche Foodtruck wirklich gelöscht werden? Dies kann <b>nicht rückgängig</b> gemacht werden!", "YesNo", null, function () {
                // Löschen bestätigt
                truck.CompanyId = "mine";
                $http.post(apiUrl + 'truck/delete', {
                    Truck: truck
                })
                    .success(function (data, status, headers, config) {
                        $scope.refreshTruckData();
                    })
                    .error(function (data, status, headers, config) {
                        redirectOnError(status, null, true);
                    });
            }, null)
        };
        $scope.openStartDate = function ($event, location) {
            $event.preventDefault();
            $event.stopPropagation();

            location.startDateOpened = true;
        };
        $scope.openEndDate = function ($event, location) {
            $event.preventDefault();
            $event.stopPropagation();

            location.endDateOpened = true;
        };
        $scope.assignTime = function (time, target) {
            target = time;
        };
        $scope.saveLocation = function (location, $event, followupAction) {

            location.latitude = null;
            location.longitude = null;

            var address = location.zip + " " + location.city + " ," + location.street;

            getAddr(address, function(status, res) {
                if (status == 'ok') {
                    location.latitude = res.latitude;
                    location.longitude = res.longitude;
                } else {
                    alert("Leider ist ein Fehler bei der Abfrage der Adresse über Google aufgetreten.")
                }

                if (location.latitude != null && location.longitude != null){
                    $http.post(apiUrl + 'location/search', {
                        location: location
                    })
                        .success(function (data, status, headers, config) {
                            location.city = data.city;
                            location.zip = data.zip;
                            location.street = data.street;
                            location.locationId = data.id;
                            location.name = data.name;
                            $scope.addTimeToDate(location.startDate, location.startTime);
                            $scope.addTimeToDate(location.endDate, location.endTime);
                            showAlert("warn", "Es gibt ungespeicherte Änderungen", undefined, $($event.target).closest('.simpleForm'));
                            followupAction();
                        })
                        .error(function (data, status, headers, config) {
                            if (status == 404) {
                                // Nicht gefundern, ergo: Neu.
                                $scope.addTimeToDate(location.startDate, location.startTime);
                                $scope.addTimeToDate(location.endDate, location.endTime);
                                location.name = location.city + ',' + location.street;
                                $scope.oldLocations.push(location);
                                showAlert("warn", "Es gibt ungespeicherte Änderungen", undefined, $($event.target).closest('.simpleForm'));
                                followupAction();
                            } else {
                                redirectOnError(status);
                            }
                        });
                } else {
                    showAlert("warn", "Leider konnten keine Adressdaten zur eingegebenen Adresse abrufen werden. Bitte prüfe die Anschrift.", undefined, $($event.target).closest('.simpleForm'));
                }
            });


        };
    $scope.editLocation = function (location, $event) {
        $scope.saveLocation(location, $event, function () {
            location.editMode = false;

        });
    };
        $scope.removeLocation = function (location,$event) {
            var index = $scope.truckDetail.locations.indexOf(location);
            $scope.truckDetail.locations.splice(index, 1);
            showAlert("warn", "Es gibt ungespeicherte Änderungen", undefined, $($event.target).closest('.simpleForm'));
        };

        $scope.addLocation = function (location, $event) {
            $scope.saveLocation(location, $event, function () {
                $scope.truckDetail.locations.push(location);
                //$scope.addToLocationList(location);
                $scope.initNewLocation();
            });
        };

        $scope.addTimeToDate = function (datetime, time) {
            var portions = time.trim().split(':');
            if (portions.length == 2) {
                datetime.setHours(portions[0]);
                datetime.setMinutes(portions[1]);
            }
        };
        $scope.initNewLocation = function () {
            $scope.truckDetail.newLocation = {
                startTime: "08:00",
                endTime: "22:00",
                startDate: new Date(),
                endDate: new Date()
            };
        };
        $scope.uploadTruckImage=function(truck, companyId, imageId) {
            var modalInstance = $modal.open({
                animation: true,
                templateUrl: './popups/imageUploadModal.php?pictureType=truck&id='+truck.id+'&companyId='+companyId+'&imageId='+imageId,
                controller: 'ImageUploadController',
                windowClass: 'imageUploadWin'
            });

            modalInstance.result.finally(function(){
                $scope.refreshTruckData();
            });
        };
        $scope.init()
});
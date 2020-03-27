/**
 * Created by Tomato on 27.05.2015.
 */


foodtruckApp.controller('PdfController', function ($scope, $http, $modalInstance) {
    $scope.truckAwesome = '<i class="fa fa-truck"></i> <br/>',
        $scope.cardAwesome = '<i class="fa fa-cutlery"></i> <br/>',
        $scope.companyAwesome = '<i class="fa fa-building"></i> <br/>',
        $scope.exportStarted=false,
        $scope.selectedBlocks = [],
        $scope.nonSelectedBlocks = [],
        $scope.companyBlocks = [
            {
                id: 0,
                short: "header",
                name: $scope.companyAwesome + "Allgemeine Infos"
            },
            {
                id: 0,
                short: "contact",
                name: $scope.companyAwesome + "Kontaktdaten"
            },
            {
                id: 0,
                short: "social",
                name: $scope.companyAwesome + "Soziale Netzwerke & Links"
            },
        ],

        $scope.getExportType = function () {
            var exportType = searchModal("exportType");
            if (exportType === undefined) {
                exportType = "company"
            }
            return exportType;
        },
        $scope.getId = function () {
            return searchModal("id");
        },
        $scope.company = {},
        $scope.getCompany = function () {
            $http.get(apiUrl + 'company/get/mine/true').
                success(function (data, status, headers, config) {
                    //$scope.aboutMe={Id:0};
                    $scope.company = data;
                    $scope.loadCompanyBlocks();
                    $scope.getTrucks();
                }).
                error(function (data, status, headers, config) {
                    redirectOnError(status);
                });
        },
        $scope.getTrucks = function () {
            $http.get(apiUrl + 'company/getAllTrucks/mine/true').
                success(function (data, status, headers, config) {
                    //$scope.aboutMe={Id:0};
                    $scope.company.Trucks = data;
                    $scope.loadTruckBlocks();
                }).
                error(function (data, status, headers, config) {
                    redirectOnError(status);
                });
        },
        $scope.loadCompanyBlocks = function () {
            $scope.removeAllBlocks($scope.selectedBlocks);
            $scope.removeAllBlocks($scope.nonSelectedBlocks);
            if ($scope.getExportType() === "company") {
                $scope.addCompanyBlocks($scope.selectedBlocks);
            } else {
                $scope.addCompanyBlocks($scope.nonSelectedBlocks);
            }
        },
        $scope.loadTruckBlocks = function () {
            switch ($scope.getExportType()) {
                case "company":
                    $scope.addTruckBlocks();
                    $scope.addCardBlocks();
                    break;
                case "truck":
                    $id = $scope.getId();
                    $scope.addTruckBlocks($id);
                    $scope.addCardBlocksForTruck($id);
                    break;
                case "card":
                    $id = $scope.getId();
                    $scope.addTruckBlocks(-1);  // Alle Trucks nach "nonSelected"
                    $scope.addCardBlocks($id);
                    break;
            }
        },

        $scope.addCompanyBlocks = function (target) {
            $scope.companyBlocks.forEach(function (block) {
                target.push(block);
            });
        },
        $scope.addTruckBlocks = function (id) {
            $scope.company.Trucks.forEach(function (truck) {
                var target = ((id === truck.id) ? $scope.selectedBlocks : $scope.nonSelectedBlocks);
                target.push({
                    id: truck.id,
                    short: "truck",
                    name: $scope.truckAwesome + truck.name
                });
            });
        },
        $scope.addCardBlocks = function (id, truckId) {
            $scope.company.Trucks.forEach(function (truck) {
                truck.AssignedCards.forEach(function (card) {
                    var showCard = false;
                    if (id !== undefined) {
                        showCard = (id === card.id);
                    }
                    if (truckId !== undefined) {
                        showCard |= (truckId === truck.id);
                    }

                    var target = (showCard ? $scope.selectedBlocks : $scope.nonSelectedBlocks);
                    target.push({
                        id: card.id,
                        short: "card",
                        name: $scope.cardAwesome + card.name
                    });
                });
            });

            // Jetzt noch alle Karten, die KEINEM Truck zugeordnet sind:
            $scope.company.Cards.forEach(function(card) {
                var unselectedItem = $.grep( $scope.nonSelectedBlocks, function(e){ return e.short=='card' && e.id == card.id; });
                var selectedItem = $.grep( $scope.selectedBlocks, function(e){ return e.short=='card' && e.id == card.id; });
                if (selectedItem.length===0 && unselectedItem.length===0) {
                    var target = (id===card.id ? $scope.selectedBlocks : $scope.nonSelectedBlocks);
                    target.push({
                        id: card.id,
                        short: "card",
                        name: $scope.cardAwesome + card.name
                    });
                }
            });

        },
        $scope.addCardBlocksForTruck = function (id) {
            $scope.addCardBlocks(undefined, id);
        },
        $scope.removeAllBlocks = function (target) {
            target = [];
        },


        $scope.init = function () {
            console.log("init PdfController");
            $scope.getCompany();
        },
        $scope.onDrop = function (what) {
        },
        $scope.addPdfBlock = function (item) {
            var index = $scope.nonSelectedBlocks.indexOf(item);
            $scope.nonSelectedBlocks.splice(index, 1);
            $scope.selectedBlocks.push(item);
            console.log("block hinzugefügt");
        },
        $scope.removePdfBlock = function (item) {
            var index = $scope.selectedBlocks.indexOf(item);
            $scope.selectedBlocks.splice(index, 1);
            $scope.nonSelectedBlocks.push(item);
            console.log("block entfernt");
        },

        $scope.startExport=function() {
            $scope.exportStarted=true;
           var config={
               FilePrefix:$scope.getExportType()+$scope.getId(),
               DisplayTrucks:[],
               DisplayCards:[]
           };

            var index=0;
            $scope.selectedBlocks.forEach(function (block) {
                index++;
               if (block.short==="header") {
                   config.Header=index;
               } else if (block.short==="contact") {
                    config.Contact=index;
               } else if (block.short==="social") {
                   config.Social=index;
               } else if (block.short==="truck") {
                   block.Show=index;
                   config.DisplayTrucks.push(block);
               } else if (block.short==="card") {
                   block.Show=index;
                   config.DisplayCards.push(block);
               }
            });
            $http.post(apiUrl + 'company/export/mine/pdf', {
                Config: config
            }).
                success(function (data, status, headers, config) {
                   window.location.href=apiUrl+ data;
                    $scope.exportStarted=false;
                }).
                error(function (data, status, headers, config) {
                    alert("Export fehlgeschlagen!");
                    $scope.exportStarted=false;
                });
        } ,


        $scope.dragControlListeners = {

            accept: function (sourceItemHandleScope, destSortableScope) {
                return true
            }, //override to determine drag is allowed or not. default is true.
            itemMoved: function (event) {
            },
            orderChanged: function (event) {
            },
            containment: '#board'//optional param.
        }
,
    $scope.init()
});
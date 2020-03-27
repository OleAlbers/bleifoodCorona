foodtruckFrontendApp.factory('truckService', function($http) {
    return {
        TruckImageSize:"250x250",
        BigItemImageSize:"250x250",
        ImagePopupSize:"500x500",
        TruckImagePath:"./public/img/__COMPANY__/truck/__ID_____SIZE__.jpg",
        ItemImageSize:"75x75",
        ItemImagePath:"./public/img/__COMPANY__/food/__ID_____SIZE__.jpg",
        ItemNoPhotoPath:"./html/assets/flavours/ribsndogs/images/content/logo-small.png",
        FoodColumns:0,
            Truck : {},
        Init:function(id) {
            this.Truck={id:id};
            this.GetTruck();
            this.Categories= [
                { name:"Desserts",icon:"icon-2"},
                { name:"Hauptgerichte",icon:"icon-1"},
                { name:"Getränke",icon:"icon-5"}
            ];
            try {
                $('#social-share').dcSocialShare(
                    {
                        buttons: 'plusone,facebook,twitter',
                    }
                );
                $('.pluginButtonLabel').hide();
            } catch (e) {

            }
        },
        GetNetworkIcon:function(name) {
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
        },
        GetTruck:function() {
            var base=this;
            $http.get(apiUrl + "truck/get/"+this.Truck.id)
            .success(function(data,status,headers,config){
                base.Truck=data;
                base.Truck.Images.forEach(function(image) {
                    if (image.imageId===undefined) {
                        item.imagesrc = base.ItemNoPhotoPath.replace("__SIZE__", base.TruckImageSize);
                    } else {
                        image.src = base.TruckImagePath.replace("__COMPANY__", base.Truck.companyId).replace("__ID__", image.imageId).replace("__SIZE__", base.TruckImageSize);
                    }
                });
                base.Truck.AssignedCards.forEach(function(card) {
                    base.GetCard(card);
                });
            })
            .error(function (data, status, headers, config) {
                //    redirectOnError(status);
            });
    },

    SetItemPath:function(item) {
        if (item.imageId==="undefined") {
            item.imagesrc = this.ItemNoPhotoPath.replace("__SIZE__", this.ItemImageSize);
            item.imagepopup=this.ItemNoPhotoPath.replace("__SIZE__", this.ImagePopupSize);
            item.imagebig=this.ItemNoPhotoPath.replace("__SIZE__", this.BigItemImageSize);
        } else {
            item.imagesrc = this.ItemImagePath.replace("__COMPANY__", this.Truck.companyId).replace("__ID__", item.imageId).replace("__SIZE__", this.ItemImageSize);
            item.imagepopup=this.ItemImagePath.replace("__COMPANY__", this.Truck.companyId).replace("__ID__", item.imageId).replace("__SIZE__", this.ImagePopupSize);
            item.imagebig=this.ItemImagePath.replace("__COMPANY__", this.Truck.companyId).replace("__ID__", item.imageId).replace("__SIZE__", this.BigItemImageSize);

        }
    },
    GetColumnClass:function() {
        switch (this.FoodColumns) {
            case 1:
                return "col-md-12";
            break;
            case 2:
                return "col-md-6";
            break;
            case 3:
                return "col-md-4";
            break;
        }
    },
    GetCard:function(card) {
        var base=this;
        $http.get(apiUrl + "card/get/"+card.id)
            .success(function(data,status,headers,config){
                card.Items=data.Items;
                card.Groups=data.Groups;
                card.Groups.forEach(function(group) {
                    group.Items.forEach(function(item) {
                        base.SetItemPath(item);
                        if (item.price!==null && item.price!==undefined) {
                            item.price=item.price.replace('.',',');
                        }
                    });
                    if (group.Items!==undefined) {
                        group.Items[0].First=true;
                    }
                    if (group.type==="Food") {
                        base.Categories[1].Items=group.Items;
                        base.FoodColumns++;
                    } else if (group.type==="Drink") {
                        base.Categories[2].Items=group.Items;
                        base.FoodColumns++;
                    } else if (group.type==="Sweets") {
                        base.Categories[0].Items=group.Items;
                        base.FoodColumns++;
                    }
                });
                card.Items.forEach(function(item) {
                    base.SetItemPath(item);
                });
            })
            .error(function (data, status, headers, config) {
                //    redirectOnError(status);
            });

    }

    };
});

foodtruckFrontendApp.controller('TruckController', function ($scope, $http, $location, truckService, angularLoad) {
    var marker;
    var currentDateSelected=0;
    var markersArray = [];
    var bounds = new google.maps.LatLngBounds();
    var truckId = search("id");
    var itemId = search("param");


    $scope.currentDateWord='Heute';
    $scope.currentDateSelected=0;

    $scope.Service=truckService;

    $scope.Init=function() {
        angularLoad.loadCSS("public/css/social.css");
        changeBackground("background-1.jpg");
        $scope.Service.Init(search("id"));
        if (itemId!==undefined && itemId!==null) {
        //    $location.hash(itemId);
        }
        getTruckLocation(truckId,currentDateSelected);
    };

    $scope.moveDate = function (pointer) {

        if(pointer=='-'){
            if($scope.currentDateSelected!=0){
                $scope.currentDateSelected--;
            }
        }else{
            if($scope.currentDateSelected<8) {
                $scope.currentDateSelected++;
            }
        }
        //console.log($scope.currentDateSelected);
        $scope.setDateWord();
        getTruckLocation(truckId,$scope.currentDateSelected);
    },
        $scope.ShopImagePopup=function(item) {

        var modalInstance = $modal.open({
            animation: true,
            templateUrl: 'popups/image.html',
            controller: 'ImageControllerModal',
            windowClass: 'imageWin',
            backdropClass: 'imageBackdrop',
            size: 'lg',
            resolve: {
                url: function () {
                    return item.imagepopup ;
                }
            }
        });
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
    };
    var getFutureTruckLocation=function(truckId, day) {
        var base=this;
        $http.get(apiUrl+'truck/getFutureTruckLocation/'+truckId+'/'+day).success(function(data) {
            //console.log(data.truckLocations);
            if (data.truckLocations===undefined || data.truckLocations.length===0) {
                console.log("Auch keine zukünftigen Termine");
            } else {
                $scope.futurePlaces=data.truckLocations;
            }
        });
    };

    var addZero=function (number) {
        if (number>9) return number;
        return "0"+number;
    };
    $scope.germanDate=function (date) {
        var dateLeftRight=date.split(" ");
        var dateParts=dateLeftRight[0].split('-');
        var timeParts=dateLeftRight[1].split(':');

        var date=new Date(dateParts[0],dateParts[1],dateParts[2],timeParts[0],timeParts[1],timeParts[2]);
        return $.datepicker.formatDate('dd.mm.yy ',date)+addZero(date.getHours())+":"+addZero(date.getMinutes());
    };

    var getTruckLocation = function(truckId, day){
        var base=this;
        $scope.futurePlaces=null;
        $scope.todaysLocations=null;
        $http.get(apiUrl+'truck/getTruckLocation/'+truckId+'/'+day).success(function(data) {
            //console.log(data.truckLocations);
            if (data.truckLocations===undefined || data.truckLocations.length===0) {
                console.log("Keine Location für Truck gefunden");
                getFutureTruckLocation(truckId, day);
            } else {
                $scope.todaysLocations=data.truckLocations;
                angular.element(document).ready(function () {
                    var mapExists=setInterval(function () {
                       if(document.getElementById('googleMapSelectedTruck')!==undefined) {
                           setMapMarker(data.truckLocations);
                           clearInterval(mapExists);
                       }
                    });
                });
            }
        });
    };


    var setMapMarker = function(location){

        var mapOptions = {
            zoom: 11,
            mapTypeId: google.maps.MapTypeId.TERRAIN,
            // zoomControl: false,
            mapTypeControl: false,
            rotateControl: false,
            scaleControl: false,
            streetViewControl:false
        };

        if (location.length>0) {

            mapOptions.center=new google.maps.LatLng(location[0].latitude, location[0].longitude);
            $scope.map = new google.maps.Map(document.getElementById('googleMapSelectedTruck'), mapOptions);
            $scope.markers = [];

            location.forEach(function (truckLocation) {
                var myLatLng = new google.maps.LatLng(truckLocation.latitude, truckLocation.longitude);
                var marker = new google.maps.Marker({
                    position: myLatLng,
                    map: $scope.map,
                    icon: 'html/assets/images/marker-icon.png',
                    title: truckLocation.name,
                    zIndex: parseInt(truckLocation.locationId),
                    animation: 2,
                    truckId: parseInt(truckLocation.truckId)
                });


                bounds.extend(myLatLng);
                markersArray.push(marker);
                $scope.markers.push(marker);
                google.maps.event.addListener(marker, 'click', function(event) {        });
            });
        } else {
            console.log("Keine Koordinaten, ergo: Keine Karte");
        }

    };
});


foodtruckFrontendApp.controller('ImageControllerModal', function ($scope, $http, $modalInstance, url) {
    $scope.url;
    $scope.Init=function() {
        //changeBackground("background-1.jpg");
        $scope.url=(url);
    };

});

foodtruckFrontendApp.controller('TruckControllerModal', function ($scope, $http, $modalInstance, id, truckService) {

    var marker;
    var currentDateSelected=0;
    var markersArray = [];
    var bounds = new google.maps.LatLngBounds();
    var truckId = id;

    $scope.currentDateWord='Heute';
    $scope.currentDateSelected=0;
    $scope.isModal=true;

    $scope.Service=truckService;

    $scope.Init=function() {
        //changeBackground("background-1.jpg");
        $scope.Service.Init(id);
        getTruckLocation(truckId,$scope.currentDateSelected);
    };


    $scope.moveDate = function (pointer) {

        if(pointer=='-'){
            if($scope.currentDateSelected!=0){
                $scope.currentDateSelected--;
            }
        }else{
            if($scope.currentDateSelected<8) {
                $scope.currentDateSelected++;
            }
        }
        //console.log($scope.currentDateSelected);
        $scope.setDateWord();
        getTruckLocation(truckId,$scope.currentDateSelected);
    };
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
        };

    var getTruckLocation = function(truckId, day){
        var base=this;
        $http.get(apiUrl+'truck/getTruckLocation/'+truckId+'/'+day).success(function(data) {
            //console.log(data.truckLocations);
            if (data.truckLocations===undefined || data.truckLocations.length===0) {
                console.log("Keine Trucklocations gefunden. Suche Zukunftstermine");

            } else {
                setMapMarker(data.truckLocations);

            }
        });
    };




    var setMapMarker = function(location){

        var mapOptions = {
            zoom: 11,
            mapTypeId: google.maps.MapTypeId.TERRAIN,
            // zoomControl: false,
            mapTypeControl: false,
            rotateControl: false,
            scaleControl: false,
            streetViewControl:false
        };

        if (location.length>0) {
            var truckLocation=location[0];
            mapOptions.center=new google.maps.LatLng(truckLocation.latitude, truckLocation.longitude);
            $scope.map = new google.maps.Map(document.getElementById('googleMapSelectedTruck'), mapOptions);
            $scope.markers = [];
            var myLatLng = new google.maps.LatLng(truckLocation.latitude, truckLocation.longitude);
            var marker = new google.maps.Marker({
                position: myLatLng,
                map: $scope.map,
                icon: 'html/assets/images/marker-icon.png',
                title: truckLocation.name,
                zIndex: parseInt(truckLocation.locationId),
                animation: 2,
                truckId: parseInt(truckLocation.truckId)
            });


            bounds.extend(myLatLng);
            markersArray.push(marker);
            $scope.markers.push(marker);
            google.maps.event.addListener(marker, 'click', function(event) {        });
        } else {
            console.log("Keine Koordinaten, ergo: Keine Karte");

        }
    };

    $scope.Close=function() {
        $modalInstance.dismiss();
    };


});
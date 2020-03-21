/**
 * Created by Tomato on 26.04.2015.
 */

foodtruckApp.controller('HomeController',function($scope,$http) {

    $scope.init = function () {
        // what the heck?
        console.log("dashboard init");

        $http.get(apiUrl+'company/get/mine').
            success(function (data, status, headers, config) {
                //$scope.aboutMe={Id:0};
                $scope.aboutMe = data;
            }).
            error(function (data, status, headers, config) {
                if (status == 404) {
                    $scope.aboutMe={Id:0};
                    // Noch keine Company angelegt; Macht nix :)
                } else {
                    redirectOnError(status);
                }
            });
    },
    $scope.menu=function() {
        var submenu=search("action");
        if (submenu===undefined) {
            submenu="kontakt"
        }
        return submenu;
    },
    $scope.getRandom=function() {
        return Math.floor((Math.random()*6)+1);
    },
    $scope.menuUrl = "includes/dashboard/" + $scope.menu() + ".html?v=" + $scope.getRandom(),
    $scope.$on('$includeContentLoaded', function(event) {
        $(".bsSwitch").bootstrapSwitch();
    }),
    $scope.scrollTo = function(id) {
        var old = $location.hash();
        $location.hash(id);
        $anchorScroll();
        //reset to old to keep any additional routing logic from kicking in
        $location.hash(old);
    },
    $scope.accountLogout = function ($event) {
            console.log('accountLogout');

            $http.get(apiUrl+'login/logout').
                success(function (data, status, headers, config) {
                    if(data.logoutStatus){
                        if(data.logoutStatus == "successful"){
                            redirectOnError(401);
                        } else {
                            redirectOnError("unknown");
                        }
                    }
                }).
                error(function (data, status, headers, config) {
                    redirectOnError(status);
                });

    };
    $scope.social=function(network) {
        var result = $.grep(socialNetworks, function(e){ return e.id == network; });
        return result[0].url;
    };
    $scope.init()
});
foodtruckApp.controller('NewMailAddressController', function ($scope, $http) {


    $scope.passwordReset = function ($event) {
        console.log('passwordReset');

        $http.post(apiUrl + 'login/changeUserEmail', {
            email: $scope.login.email
        }).
            success(function (data) {
                if(data.error){
//                    console.log("data.error " + data.error);
                    showAlert("error", data.error, undefined, $($event.target).closest('.easyBox'));
                }else if (data.success){
                    showAlert("success", data.success, undefined, $($event.target).closest('.easyBox'));
                }
            })

            .error(function (data, status) {
                redirectOnError(status, $($event.target).closest('.easyBox'));
            })
    }
});
foodtruckApp.controller('ChangePasswordController', function ($scope, $http) {

    $scope.changePassword = function ($event) {

        $http.post(apiUrl + 'login/changePassword', {
            password: $scope.register.password,
            password_repeat:$scope.register.password_repeat
        }).
            success(function (data) {

                console.log(data);

                if(data.error){
                    console.log("data.error " + data.error);
                    showAlert("error", data.error, undefined, $($event.target).closest('.easyBox'));
                }else if (data.success){
                    showAlert("success", data.success, undefined, $($event.target).closest('.easyBox'));
                }else{
                    showAlert("warn", "Ein unbekanntes Ereignis ist aufgetreten.", undefined, $($event.target).closest('.easyBox'));
                }
            })

            .error(function (data, status) {
                redirectOnError(status, $($event.target).closest('.easyBox'));
            })
    }

});
foodtruckApp.controller('CompanyAccountController', function ($scope, $http) {


    $scope.deleteAccount = function ($event) {
        console.log('deleteAccount');

        $http.get(apiUrl+'login/deleteAccount').
            success(function (data) {

                console.log(data);

                if(data.error){
                    console.log("data.error " + data.error);
                    showAlert("error", data.error, undefined, $($event.target).closest('.easyBox'));
                }else if (data.success){
                    showAlert("success", data.success, undefined, $($event.target).closest('.easyBox'));
                }else{
                    showAlert("warn", "Ein unbekanntes Ereignis ist aufgetreten.", undefined, $($event.target).closest('.easyBox'));
                }
            })

            .error(function (data, status) {
                redirectOnError(status, $($event.target).closest('.easyBox'));
            })

    }
});
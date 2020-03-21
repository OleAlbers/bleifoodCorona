/**
 * Created by Lars on 28.04.2015.
 */

foodtruckFrontendApp.controller('PublicHomeController',function($scope, angularLoad) {

    $scope.init = function () {
        //console.log("Loading JS");

        //$scope.loadCss();
        // what the heck?
        console.log("init done");
    };


    $scope.menu=function() {
        var submenu=search("action");
        if (submenu===undefined) {
            submenu="fahrplan"
        }

        return submenu;
    };
    $scope.getRandom=function() {
        return Math.floor((Math.random()*6)+1);
    };
    $scope.menuUrl = "includes/publicsite/" + $scope.menu() + ".html?v=" + $scope.getRandom();
    $scope.$on('$includeContentLoaded', function(event) {
        $(".bsSwitch").bootstrapSwitch();
    });
    $scope.scrollTo = function(id) {
        var old = $location.hash();
        $location.hash(id);
        $anchorScroll();
        //reset to old to keep any additional routing logic from kicking in
        $location.hash(old);
    };
    $scope.social=function(network) {
        var result = $.grep(socialNetworks, function(e){ return e.id == network; });
        return result[0].url;
    };
    $scope.init()
});

foodtruckFrontendApp.controller('PasswordVerifyResetController', function ($scope, $http) {

    var hash;
    var username;
    $scope.init = function () {
        username=search("id");
        hash=search("param");

        if(hash===undefined || username===undefined){
            //console.log("missing mandatory url parameter");
            $scope.somePlaceholder = 'Der Link ist nicht vollständig';
            showAlert("error", 'Der Link ist nicht vollständig', undefined, $('.passwordResetMessageBox'));
        }
        $scope.resetAllowed();
    };
    $scope.resetAllowed=function() {
        var id=search("id");
        var param=search("param");

        $http.get(apiUrl + 'login/verifyPasswordReset/' + id + "/" + param).

            success(function (data, status, headers, config) {

                console.log("pwreset:"+data);
                $scope.resetIsAllowed=data;
            })
            .error(function (data, status, headers, config) {
                //redirectOnError(status, $($event.target).closest('.col-md-5'));
                $scope.resetIsAllowed=false;
            })
    };
        $scope.setNewPassword = function ($event) {

            if(hash===undefined || username===undefined){
                showAlert("error", 'Der Link ist nicht vollständig', undefined, $('.passwordResetMessageBox'));
            } else {
                $http.post(apiUrl + 'login/setNewPassword', {
                    username: username,
                    hash: hash,
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

                    .error(function (data, status, headers, config) {
                        redirectOnError(status, $($event.target).closest('.col-md-12'));
                    })
            }
        }
});
foodtruckFrontendApp.controller('RegisterLoginController', function ($scope, $http, $location) {

    //$scope.somePlaceholder = 'TEST';
    $scope.init = function () {
        console.log("RegisterLoginController init");

        $http.get(apiUrl+'login/companyAccountLoginState').
            success(function (data, status, headers, config) {
                if(data.loginState){
                    if(data.loginState == "logged in"){
                        //console.log("you are already logged in");
                        $('#companyLoginButton').hide();
                        $('#companyLogoutButton').show();
                        redirectOnError("loginSuccessFul");
                    } else {
                        $('#companyLoginButton').show();
                        $('#companyLogoutButton').hide();
                    }
                }
            }).
            error(function (data, status, headers, config) {
                //console.log("error: " + data);
            });
    },
    $scope.registerNewAccount = function ($event) {
        //console.log('registerNewAccount');

        $http.get(apiUrl+'login/companyAccountLoginState').
            success(function (data, status, headers, config) {
                if(data.loginState){
                    if(data.loginState == "logged in"){
                        //console.log("you are already logged in");
                        showAlert("error", "Vor dem Registrieren eines neuen Accounts melde dich bitte vorher über den Ausloggen Button (links) ab. ", undefined, $($event.target).closest('.col-md-5'));
                    } else {
                        $http.post(apiUrl + 'login/registerNewCompanyAccount', {
                            email: $scope.register.email,
                            password: $scope.register.password,
                            password_repeat:$scope.register.password_repeat
                        }).
                            success(function (data, status, headers, config) {

                                console.log(data);

                                if(data.error){
                                    console.log("data.error " + data.error);
                                    showAlert("error", data.error, undefined, $($event.target).closest('.col-md-5'));
                                }else if (data.success){
                                    showAlert("success", data.success, undefined, $($event.target).closest('.col-md-5'));
                                }else{
                                    showAlert("warn", "Ein unbekanntes Ereignis ist aufgetreten.", undefined, $($event.target).closest('.col-md-5'));
                                }


                            })

                            .error(function (data, status, headers, config) {
                                redirectOnError(status, $($event.target).closest('.col-md-5'));
                            })
                    }
                }
            }).
            error(function (data, status, headers, config) {
                //console.log("error: " + data);
            });

    },
    $scope.accountLogin = function ($event) {
        console.log('accountLogin');

        $http.post(apiUrl + 'login/loginCompanyAccount', {
            email: $scope.login.email,
            password: $scope.login.password
        }).
            success(function (data, status, headers, config) {

                console.log(data);

                if(data.error){
                    console.log("data.error " + data.error);
                    showAlert("error", data.error, undefined, $($event.target).closest('.col-md-5'));
                }else if (data.success){
                    redirectOnError("loginSuccessFul");
                }else{
                    showAlert("warn", "Ein unbekanntes Ereignis ist aufgetreten.", undefined, $($event.target).closest('.col-md-5'));
                }


            })

            .error(function (data, status, headers, config) {
                redirectOnError(status, $($event.target).closest('.col-md-5'));
            })

    },
    $scope.accountLogout = function ($event) {
        console.log('accountLogout');

        $http.get(apiUrl+'login/logout').
            success(function (data, status, headers, config) {
                if(data.logoutStatus){
                    if(data.logoutStatus == "successful"){
                        //console.log("you are logged out");
                        $('#companyLoginButton').show();
                        $('#companyLogoutButton').hide();
                    } else {
                        $('#companyLoginButton').hide();
                        $('#companyLogoutButton').show();
                    }
                }
            }).
            error(function (data, status, headers, config) {
                console.log("error: " + data);
            });

    },
    $scope.init()
});

foodtruckFrontendApp.controller('EmailVerificationController', function ($scope, $http) {

    $scope.init = function () {
        // what the heck?
        //console.log("emailVerifycation");
        var action=search("action");
        var id=search("id");
        var param=search("param");

        //$scope.somePlaceholder = 'TEST';

        if(action===undefined || id===undefined || param===undefined){
            //console.log("missing mandatory url parameter");
            $scope.somePlaceholder = 'Der Link ist nicht vollständig';
        } else {
            //console.log("action: " + action + " id: " + id + " param: " + param);
            $http.get(apiUrl + 'login/verifyNewCompanyAccount/' + id + "/" + param).

                success(function (data, status, headers, config) {

                    console.log(data);

                    if(data.error){
                        console.log("data.error " + data.error);
                        $scope.somePlaceholder = data.error;
                    }else if (data.success){
                        $scope.somePlaceholder = data.success;
                        $("#loginAfterVerify").fadeIn();
                    }else{
                        $scope.somePlaceholder = 'Ein unbekanntes Ereignis ist aufgetreten.';
                    }


                })

                .error(function (data, status, headers, config) {
                    //redirectOnError(status, $($event.target).closest('.col-md-5'));
                    $scope.somePlaceholder = 'Ein Fehler im Aufruf der Seite ist aufgetreten.';
                })
        }

    },
    $scope.init()

});

foodtruckFrontendApp.controller('PasswordResetController', function ($scope, $http) {


    $scope.passwordReset = function ($event) {
        console.log('passwordReset');

        $http.post(apiUrl + 'login/requestPasswordReset', {
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

        .error(function (data, status, headers, config) {
            redirectOnError(status, $($event.target).closest('.col-md-12'));
        })
    };

});

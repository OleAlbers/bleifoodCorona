foodtruckApp.controller('CompanyController', function ($scope, $http, $location) {
    $scope.init = function () {
        console.log("init");
        $http.get(apiUrl+'company/get/mine').
            success(function (data, status, headers, config) {
                $scope.aboutMe = data;
                $scope.aboutMe.Published = data.IsPublished == '1';
                if (data.SocialMedia!==undefined && data.SocialMedia!==null) {
                    angular.forEach(data.SocialMedia, function(value, key) {
                        if (value.name==='Twitter') {
                            $scope.aboutMe.Twitter=value.url;
                            if ($scope.aboutMe.Twitter=='') {
                                $scope.aboutMe.Twitter='https://twitter.com/';
                            }
                        } else if (value.name==='Facebook') {
                            $scope.aboutMe.Facebook=value.url;
                            if ($scope.aboutMe.Facebook=='') {
                                $scope.aboutMe.Facebook='https://facebook.com/';
                            }
                        } else if (value.name==='Google+') {
                            $scope.aboutMe.Google=value.url;
                            if ($scope.aboutMe.Google=='') {
                                $scope.aboutMe.Google='https://plus.google.com/';
                            }
                        } else if (value.name==='Website') {
                            $scope.aboutMe.Url=value.url;
                        }
                    });
                }
                if (data.Contacts!==undefined && data.Contacts!==null) {
                    angular.forEach(data.Contacts, function(value, key) {
                        if (value.mode==='Private') {
                            $scope.aboutMe.PrivateContact=value;
                        } else {
                            $scope.aboutMe.PublicContact=value;
                            $scope.aboutMe.PublicContact.Published=value.IsPublished=='1';
                        }
                    });
                }
            }).
            error(function (data, status, headers, config) {
                if (status == 404) {
                    // Noch keine Company angelegt; Macht nix :)
                } else {
                    redirectOnError(status);
                }
            });
    },
        $scope.saveData = function ($event) {
            clearAlert($($event.target).closest('.easyBox'));
            // Benötigte Permissions: Employee
            $http.post(apiUrl+'company/set', {
                Id: 'mine',
                Name: $scope.aboutMe.Name,
                Street: $scope.aboutMe.Street,
                Zip: $scope.aboutMe.Zip,
                City: $scope.aboutMe.City,
                Status: $scope.aboutMe.Published
            }).
                success(function (data, status, headers, config) {
                    $scope.aboutMe.Id=data.Id;  // ID zuweisen. Falls INSERT werden danach die anderen DIVs sichtbar
                    showAlert("success", "Speichern erfolgreich", undefined, $($event.target).closest('.easyBox'));
                }).
                error(function (data, status, headers, config) {
                    redirectOnError(status, $($event.target).closest('.easyBox'));
                });
        },
        $scope.savePublic=function($event) {
            clearAlert($($event.target).closest('.easyBox'));
            // Benötigte Permissions: Employee
            $http.post(apiUrl+'company/setAddress', {
                CompanyId: 'mine',
                Name: "Kundensupport",
                Phone:$scope.aboutMe.PublicContact.phone,
                Fax:$scope.aboutMe.PublicContact.fax,
                Mail:$scope.aboutMe.PublicContact.mail,
                IsPublished:true,
                Id:$scope.aboutMe.PublicContact.id,
                Private:false

            }).
                success(function (data, status, headers, config) {
                    showAlert("success", "Speichern erfolgreich", undefined, $($event.target).closest('.easyBox'));
                }).
                error(function (data, status, headers, config) {
                    redirectOnError(status, $($event.target).closest('.easyBox'));
                });
        },

        $scope.saveSocialData=function($event) {
            clearAlert($($event.target).closest('.easyBox'));
            // Benötigte Permissions: Employee
            $http.post(apiUrl+'company/setSocial', {
                CompanyId: 'mine',
                facebookUrl:$scope.aboutMe.Facebook,
                googleUrl:$scope.aboutMe.Google,
                twitterUrl:$scope.aboutMe.Twitter,
                websiteUrl:$scope.aboutMe.Url
            }).
                success(function (data, status, headers, config) {
                    showAlert("success", "Speichern erfolgreich", undefined, $($event.target).closest('.easyBox'));
                }).
                error(function (data, status, headers, config) {
                    redirectOnError(status, $($event.target).closest('.easyBox'));
                });
        },
        $scope.aboutMe,
        $scope.init()

});
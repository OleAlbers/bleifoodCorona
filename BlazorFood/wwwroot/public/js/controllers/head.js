foodtruckFrontendApp.controller('HeadController', function ($scope,angularLoad) {


    $scope.loadCss=function() {
        angularLoad.loadCSS("html/assets/css/font-awesome.css");

       //     angularLoad.loadScript("public/js/helper.js");


        //angularLoad.loadCSS("public/css/social.css");
    };

    $scope.loadCss();
});

foodtruckApp.controller('YtController', function ($scope, $http, $modalInstance, $sce, id) {
    $scope.id=id,
        $scope.getUrl=function() {
            return $sce.trustAsResourceUrl("https://www.youtube.com/embed/"+$scope.id+"?rel=0&amp;showinfo=0&amp;autoplay=1");
        }
});
/**
 * Created by lbo on 07.07.15.
 */

foodtruckApp.controller('ImageUploadController', function ($scope, $http, $modalInstance) {
    $scope.enableCrop=true;
    $scope.size='medium';
    $scope.type='circle';
    /*$scope.type='square';*/
    $scope.imageDataURI='';
    $scope.resImageDataURI='';
    $scope.resImgFormat='image/jpeg';
    $scope.resImgQuality=0.8;
    $scope.selMinSize=100;
    //$scope.aspectRatio=1.2;
    $scope.onChange=function($dataURI) {
        console.log('onChange fired');
    };
    $scope.onLoadBegin=function() {
        console.log('onLoadBegin fired');
    };
    $scope.onLoadDone=function() {
        console.log('onLoadDone fired');
    };
    $scope.onLoadError=function() {
        console.log('onLoadError fired');
    };
    $scope.getPictureType = function () {
        return searchModal("pictureType");
    };
    $scope.getTypeId = function () {
        return searchModal("id");
    };
    $scope.getImageId = function () {
        return searchModal("imageId");
    };
    $scope.getCompanyId = function () {
        return searchModal("companyId");
    };

    var handleFileSelect=function(evt) {
        console.log('handleFileSelect');
        var file=evt.currentTarget.files[0];
        console.log('file');
        var reader = new FileReader();
        console.log('reader');
        reader.onload = function (evt) {
            $scope.$apply(function($scope){
                $scope.imageDataURI=evt.target.result;
            });
        };
        reader.readAsDataURL(file);
    };
    $(document).on('change','#fileInput',handleFileSelect);

    $scope.$watch('resImageDataURI',function(){
        console.log('Res image: ', $scope.resImageDataURI);
    });

    $scope.imageUpload = function ($event) {

        console.log('imageUpload');
        console.log('pictureType: ' + $scope.pictureType);
        console.log('imageId: ' + $scope.imageId);
        console.log('typeId: ' + $scope.typeId);

        $http.post('./api/image/set', {
            image: $scope.resImageDataURI,
            quality: $scope.resImgQuality,
            picType: $scope.pictureType,
            typeId: $scope.typeId,
            imageId: $scope.imageId

        }).
            success(function (data, status, headers, config) {
                //console.log("data: " + data);
                //console.log("status: " + status);
                //console.log("headers: " + headers);
                if(data.error){
                    //console.log("data.error " + data.error);
                    showAlert("error", data.error, undefined, $('#imageUploadModalMessage'));
                }else if (data.success){
                    //console.log("data.success " + data.success);
                    $scope.imageId = data.imageId;
                    $scope.imageStateNew = data.imageStateNew;
                    showAlert("success", "Das Bild wurde erfolgreich hochgeladen", undefined, $('#imageUploadModalMessage'));
                }else{
                    //console.log("else: " + data);
                }

            })

            .error(function (data, status, headers, config) {
                console.log("error: " + data);
            })

    },
        $scope.imageDelete = function ($event) {

            console.log('imageDelete');
            console.log('pictureType: ' + $scope.pictureType);
            console.log('imageId: ' + $scope.imageId);
            console.log('typeId: ' + $scope.typeId);

            $http.post('./api/image/delete', {
/*                image: $scope.resImageDataURI,
                quality: $scope.resImgQuality,
*/
                picType: $scope.pictureType,
                typeId: $scope.typeId,
                imageId: $scope.imageId

            }).
                success(function (data, status, headers, config) {
                    //console.log("data: " + data);
                    //console.log("status: " + status);
                    //console.log("headers: " + headers);
                    if(data.error){
                        //console.log("data.error " + data.error);
                        showAlert("error", data.error, undefined, $('#imageUploadModalMessage'));
                    }else if (data.success){
                        //console.log("data.success " + data.success);
                        $scope.imageId = data.imageId;
                        showAlert("success", "Das Bild wurde gelöscht", undefined, $('#imageUploadModalMessage'));
                    }else{
                        //console.log("else: " + data);
                    }


                })

                .error(function (data, status, headers, config) {
                    console.log("error: " + data);
                })

        },

    $scope.init = function () {
        console.log("init ImageUploadController");
        $scope.pictureType=$scope.getPictureType();
        $scope.imageId = $scope.getImageId();
        $scope.typeId=$scope.getTypeId();
        $scope.companyId=$scope.getCompanyId();

        if($scope.imageId != null||$scope.imageId == 'undefined'){
            $scope.imageDataURI="public/img/"+$scope.companyId+"/"+$scope.pictureType+"/"+$scope.imageId+"_org.jpg";
        }

    }

    //$scope.init();
});
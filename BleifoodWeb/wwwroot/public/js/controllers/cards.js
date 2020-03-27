

foodtruckApp.controller('CardsController', function ($scope, $http, $location, $anchorScroll, $modal) {
    $scope.cards=[];
        $scope.euro=/^\d+[,]?\d{0,2}$/;
        $scope.allGroups=[],
        $scope.init=function() {
            $scope.ShowNewCard=false;
            $scope.refreshCards();
            $scope.refreshGroups();
        },
        $scope.refreshGroups=function() {
            if ($scope.allGroups.length===0) {
                $http.get(apiUrl + "card/getGroups")
                    .success(function(data,status,headers,config){
                        $scope.allGroups=data;
                        $scope.filterGroups();
                    })
                    .error(function (data, status, headers, config) {
                        redirectOnError(status);
                    });
            } else {
                $scope.filterGroups();
            }
        },
        $scope.changeCategory=function(category) {
          $scope.filterGroups();
        },
        $scope.filterGroups=function() {
            if ($scope.allGroups.length>0) {
                if ($scope.cardEdit!==undefined && $scope.cardEdit!==null) {
                    if ($scope.cardEdit.Show===true || $scope.cardEdit.ShowEdit===true) {
                        // Leere Gruppen kicken:
                        for(var i =  $scope.cardEdit.Groups.length - 1; i >= 0; i--) {
                            if( $scope.cardEdit.Groups[i].Items.length === 0) {
                                $scope.cardEdit.Groups.splice(i, 1);
                            }
                        }

                        // Neue Gruppen hinzufügen:
                        $scope.allGroups.forEach(function (group) {
                            if (group.foodcategoryId===null || $scope.categorySelected($scope.cardEdit,group.foodcategoryId)) {
                                if (!$scope.groupAlreadyExists($scope.cardEdit,group.id)) {
                                    group.Items=[];
                                    $scope.cardEdit.Groups.push(group);
                                }
                            }
                        });
                    }
                }
            }
        },
        $scope.categorySelected=function(card, categoryId) {
            var foundIt=false;
          card.FoodCategories.forEach(function(category) {
              if (category.id===categoryId && category.isChecked) {
                  foundIt=true;
                  return;
              }
          });
            return foundIt;
        },
        $scope.groupAlreadyExists=function(card, groupId) {
            var foundIt=false;
            card.Groups.forEach(function(group){
                if (group.id===groupId) {
                    foundIt=true;
                    return;
                }
            });
            return foundIt;
        },

        $scope.editCard=function(card) {
            card.Loading=true;
            $scope.ShowNewCard=false;
            $scope.cards.forEach(function (card) {
                card.ShowEdit = false;
            });
            $http.get(apiUrl + 'card/get/'+card.id+'/true')
                .success(function (data, status, headers, config) {
                    var cardIndex=-1;
                    for (i=0; i<$scope.cards.length;i++) {
                        if ($scope.cards[i].id==data.id) {
                            cardIndex=i;
                            break;
                        }
                    }

                    $scope.cards[cardIndex]=data;
                    $scope.cards[cardIndex].FoodCategories=[];
                    $scope.FoodCategories.forEach(function (category) {
                        var savedCategory = $.grep(data.Categories, function(e){ return e.foodcategoryId === category.id; });
                        category.isChecked=savedCategory.length>0;
                        $scope.cards[cardIndex].FoodCategories.push(category);
                    });

                    $scope.cards[cardIndex].Groups.forEach(function(group) {
                        group.Items.forEach(function (item) {
                            if (item.price !== null && item.price !== undefined) {
                                item.price = item.price.replace('.', ',');
                            }
                        });
                    });

                    $scope.cardEdit=$scope.cards[cardIndex];
                    $scope.cardEdit.ShowEdit=true;
                    $scope.cardEdit.FormName='editForm'+$scope.cardEdit.id;
                    $scope.cardEdit.Published = $scope.cardEdit.IsPublished == "1";
                    $scope.cardEdit.newFood={};
                    $scope.refreshGroups();
                    card.Loading=false;
                })
                .error(function (data, status, headers, config) {
                    redirectOnError(status);
                });
        },
        $scope.editItem=function(item) {
            console.log(item);
            item.editMode=!item.editMode;

            var imagePath = "public/img/no_photo.png";
            if(item.imageId != 'undefined'){
                imagePath = "public/img/"+item.companyId+"/food/"+item.imageId+"_150x150.jpg";
            }

            item.edit={id:item.id, name:item.name, description:item.description, imageId:item.imageId, imagePath:imagePath, companyId: item.companyId, price:item.price};
        },
        $scope.refreshCards = function (loadlast) {
            $http.get(apiUrl + 'card/getAll/mine/true')
                .success(function (data, status, headers, config) {
                    $scope.cards = data;
                    $scope.cards.forEach(function (card) {
                        card.ShowEdit = false;
                        card.Published = card.IsPublished == "1";
                        card.Loading=false;

                    });
                    if (loadlast!==undefined && loadlast) {
                        $scope.editCard($scope.cards[$scope.cards.length-1]);
                    }
                })
                .error(function (data, status, headers, config) {
                    if (status == 404) {
                        // Noch keine Karten angelegt; Macht nix :)
                    } else {
                        redirectOnError(status);
                    }
                });
            $http.get(apiUrl + 'resource/getFoodCategories')
                .success(function (data, status, headers, config) {
                    $scope.FoodCategories=data;
                })
                .error(function (data, status, headers, config) {
                    redirectOnError(status);    // Keine Kategorien? Serious Error
                });
        },
        $scope.hasFav=function(card) {
            if (card===undefined){
                return false;
            }
            var hasFav=false;
            card.Groups.forEach(function(group) {
                group.Items.forEach(function(item) {
                    if (item.favorite && item.imageId!==undefined && item.imageId!=="undefined") {
                        hasFav= true;
                    }
                });
            }) ;
            if (!hasFav) {
                $scope.cardEdit.Published=false;
            }
            return hasFav;
        },
        $scope.createNewCard=function() {
            $scope.cards.forEach(function(card) {
               card.ShowEdit=false;
            });

            $scope.ShowNewCard=true;
            $scope.cardEdit={
                    Show: true,
                    FoodCategories: $scope.FoodCategories,
                     Items:[],
                    Groups: [
                        {name:"Speisen",description:"",Items:[]},
                        {name:"Getränke",description:"",Items:[]}
                    ],
                    newFood: {},
                    id: null,
                    FormName:'createForm'
            };
            $scope.refreshGroups();
        },
        $scope.removeItem=function(target, card, $event) {
            // aus Gruppen raus:

            card.Groups.forEach(function(group) {
                var foundItem = $.grep(group.Items, function(e){ return e.id === target.id; });
                if (foundItem.length>0) {
                    var groupIndex=group.Items.indexOf(foundItem[0]);
                    group.Items.splice(groupIndex, 1);
                }
            });
            // aus HAuptebene raus:
            var foundItem=$.grep(card.Items, function(e){ return e.id === target.id; });
            if (foundItem.length>0) {
                var cardIndex=card.Items.indexOf(foundItem[0]);
                card.Items.splice(cardIndex,1);
            }

            if ($event!==undefined) {
                showAlert("warn", "Es gibt ungespeicherte Änderungen", undefined, $($event.target).closest('.simpleForm'));
            }
        },
        $scope.favoriteItem=function(target,$event) {
            //console.log("target.id: " + target.id);
            //console.log("target.name: " + target.name);
            target.favorite=!target.favorite;


            $http.post(apiUrl + 'card/setFoodAsFavorite', {
                foodId: target.id,
                favorite: target.favorite

            }).
                success(function (data, status, headers, config) {
                    //  console.log("data: " + data);
                    //console.log("status: " + status);
                    //console.log("headers: " + headers);
                    if(data.error){
                        console.log("data.error " + data.error);
                        showAlert("error", data.error, undefined, $('.setFavoriteAlertMessage'));
                    }else if (data.success){
                        //console.log("data.success " + data.success);
                        //showAlert("success", "Das Bild wurde erfolgreich hochgeladen", undefined, $('#imageUploadModalMessage'));
                    }else{
                        //console.log("else: " + data);
                    }

                })

                .error(function (data, status, headers, config) {
                    console.log("error: " + data);
                })
        },

        $scope.editFood=function(target, $event) {
            target.name=target.edit.name;
            target.description=target.edit.description;
            target.price=target.edit.price;
            target.editMode=false;
        },
        $scope.addFood=function(target, $event) {
            if (target.newFood.category==undefined  || target.name==="" ) {
                return; // TODO: ggf. Fehlermeldung
            }
            if (!IsNullOrWhiteSpace(target.newFood.name)) {
                target.newFood.edit={name:target.newFood.name, description:target.newFood.description, price:target.newFood.price};
                target.Items.push(target.newFood);
                $scope.addFoodToGroup(target);
                target.newFood={name:"", description:"", editMode:false};

            }
        },
        $scope.addFoodToGroup=function(target) {
            var catId=target.newFood.category.id;
            target.newFood.groupId=catId;
            target.newFood.category=undefined;
          target.Groups.forEach(function(group) {
              if (group.id===catId) {
                  group.Items.push(target.newFood);
              }
          }) ;
        },
        $scope.closeCardDetail=function() {
            $scope.ShowNewCard=false;
        },
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
        },
        $scope.exportPdf=function(card) {
            var modalInstance = $modal.open({
                animation: true,
                templateUrl: 'popups/exportModal.php?exportType=card&id='+card.id,
                controller: 'PdfController',
                windowClass: 'exportWin'
            });
        },
        $scope.saveCard=function(card, $event) {

            if (card.newFood.name!=='' && card.newFood.name!==undefined) {
                showAlert("warn", "Es gibt ungespeicherte Speisen. Bitte zunächst über das \"PLUS\"-Symbol speichern", undefined, $($event.target).closest('.simpleForm'));
                return;
            }
            card.Items={};
            clearAlert($($event.target).closest('.simpleForm'));
            card.CompanyId="mine";
            card.Groups.forEach(function(group) {
                group.Items.forEach(function (item) {
                    if (item.price !== null && item.price !== undefined) {
                        item.price = item.price.replace(',', '.');
                    }
                });
            });

            $http.post(apiUrl + 'card/set', {
                Card: card
            })
                .success(function (data, status, headers, config) {
                //    $scope.refreshCards();
                    if (card.id>0) {
                        $scope.editCard(card);
                    } else {
                        $scope.refreshCards(true);
                    }
                    //showAlert("success", "Speichern erfolgreich", undefined, $($event.target).closest('.simpleForm'));
                })
                .error(function (data, status, headers, config) {
                    redirectOnError(status);
                });
        },
        $scope.deleteCard=function(card)
        {
            $http.get(apiUrl + 'card/get/'+card.id+'/true')
                .success(function (data, status, headers, config) {
                    if (data.Trucks.length>0) {
                        showPopup("error", "Karte kann nicht gelöscht werden", "Die Karte ist noch einem Truck zugeordnet!", null, null, function () {
                        });
                    } else {
                        showPopup("delete", "Speisekarte löschen?", "Soll diese Speisekarte wirklich gelöscht werden? Dies kann <b>nicht rückgängig</b> gemacht werden!", "YesNo", null, function () {
                            // Löschen bestätigt
                            card.CompanyId = "mine";
                            $http.post(apiUrl + 'card/delete', {
                                    Card: card
                                })
                                .success(function (data, status, headers, config) {
                                    $scope.refreshCards();
                                })
                                .error(function (data, status, headers, config) {
                                    redirectOnError(status, null, true);
                                });
                        }, null)
                    }
                });
        },
        $scope.uploadFoodImage=function(itemId, companyId, imageId) {
            var modalInstance = $modal.open({
                animation: true,
                templateUrl: './popups/imageUploadModal.php?pictureType=food&id='+itemId+'&companyId='+companyId+'&imageId='+imageId,
                controller: 'ImageUploadController',
                windowClass: 'imageUploadWin'
            });

            modalInstance.result.finally(function(){
                $scope.init();
            });
        },
        $scope.init();

});

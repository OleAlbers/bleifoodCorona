

foodtruckFrontendApp.controller('BlogController', function ($scope, $http, $location, $window) {
    $scope.Location=$location;
    $scope.Categories=[];
    $scope.Tags=[];
    $scope.Teasers=[];
    $scope.Article={};
    $scope.Editable=false;
  //  $scope.SuggestedImageWidth=541;

    $scope.Shares=[
        { icon: "twitter", network:"Twitter",  url:"http://twitter.com/share?url=__URL__"},
        { icon: "facebook-square", network:"Facebook", url:"http://www.facebook.com/sharer.php?u=__URL__"},
        { icon: "google-plus-square", network:"Google+", url:"https://plusone.google.com/_/+1/confirm?hl=en&url=__URL__"},
    ];

        $scope.PageCount=1;
        $scope.niceMonth=function(date) {
            if (date===undefined) {
                return null;
            }
            monthNames=["Jan","Feb","Mär","Apr","Mai","Jun","Jul","Aug","Sep","Okt","Nov","Dez"];
            var month=date.getMonth();
            return monthNames[month];
        };

        $scope.BlogByCat=function(catName) {
            return "blog/"+catName;
        };
        $scope.BlogByTag=function(tagName) {
            return "blog/all/"+tagName;
        };
        $scope.GetSocial=function() {
            var url=$location.absUrl();
            var output=[];
            $scope.Shares.forEach(function(share) {
                var outputSingle=share;
                outputSingle.url=outputSingle.url.replace("__URL__",url);
                output.push(outputSingle);
            });
            return output;
        };


        $scope.BlogByPage=function(page) {
            var cat=$scope.SearchCategory;
            var tag=$scope.SearchTag;
            if (cat===undefined || cat===null) {
                cat="all";
            }
            if (tag===undefined) {
                tag="";
            }
            return "blog/"+cat+"/"+tag+"/"+page;
        };
        $scope.SearchCategory=search("category");
        $scope.SearchTag=search("tag");
        $scope.Page=search("page");

        $scope.SetPageArray=function(pageCount) {
            $scope.PageCount=pageCount;
            $scope.PageArray=[];
            for (var i=1; i<=$scope.PageCount; i++) {
                $scope.PageArray.push(i);
            }
        };
    $scope.ShowBlog=function(teaser) {
        var title=teaser.title.replace(/[ ]/g,"-");
        title=title.replace(/[^a-zA-Z0-9\- ]/g, "");
        return "article/"+teaser.id+"/"+title;    // Title nur, ums "hübscher" zu machen für Google.
    };

    $scope.GetArticleTags=function() {
        $http.get(apiUrl + 'blog/getArticleTags/'+$scope.Id)
            .success(function (data, status, headers, config) {
                $scope.ArticleTags=data;
            });
    };
    $scope.GetArticle=function() {
        $http.get(apiUrl + 'blog/get/'+$scope.Id)
            .success(function (data, status, headers, config) {
                $scope.GetDates(data);
                $scope.Article=data;
                $scope.Article.absoluteImage=$scope.Absolute(data.image);
              //  $scope.ArticleContent=$scope.Article.content;

                $scope.SelectedCategory = $.grep($scope.Categories, function(e){ return e.id == $scope.Article.categoryId; })[0];
              $scope.GetAllAuthors();
            });
    };
    $scope.Absolute=function(url) {
        var absUrl=$location.protocol()+"://"+$location.host()+"/public/img/blog/"+url;
        return absUrl;
    };
    $scope.RemoveTag=function(tag) {
        $.each($scope.ArticleTags, function(i){
            if($scope.ArticleTags[i].name === tag.name) {
                $scope.ArticleTags.splice(i,1);
                return false;
            }
        });
    };
    $scope.GetAdminStatus=function(tag) {
        $http.get(apiUrl + 'user/isAdmin')
            .success(function (data, status, headers, config) {
                if (data===true) {
                    $scope.EnableEdit();
                }
            });
    };
    $scope.AddNewTag=function(tag) {
        $scope.ArticleTags.push({id:-1,name:tag});
    };
    $scope.AddExistingTag=function(tag) {
        $scope.ArticleTags.push(tag);
    };
    $scope.Markdown=function(source) {
        var converter = new showdown.Converter();
        html      = converter.makeHtml(source);
        return   html;
    };
    $scope.EnableEdit=function() {
        $scope.Editable=true;
        $('.blogContent').attr('contenteditable','true');
        $('.blogTeaser').attr('contenteditable','true');
        $('.blog_title a').attr('contenteditable','true');
        $('.meta_author span').attr('contenteditable','true');
        $('.meta_date').attr('contenteditable','true');
     //   $scope.GetAllAuthors();

       // $scope.ArticleContent=$scope.Markdown($scope.Article.content);

    };
    $scope.initArticle=function() {
        changeBackground("background-2.jpg");
        $scope.Id=search("id");
        if (search("param")==="edit") {
            $scope.GetAdminStatus();
        }

        $scope.GetAllCategories($scope.GetArticle);
        $scope.GetAllTags();
        $scope.GetArticleTags();
    };
    $scope.GoBack=function() {
        $window.history.back();
        return false;
    };



    $scope.UploadImage=function(id, imageId) {
        if ($scope.SelectedImageFile!==undefined) {
            if (imageId===undefined) {
                imageId=null;
            }
            $http.post(apiUrl + 'image/blogSet', {
                image: $scope.SelectedImageFile,
                blogId: id,
                imageId: imageId
            }).
                success(function (data, status, headers, config) {
                    if(data.error){
                        console.log("data.error " + data.error);
                        alert ("Fehler beim Bilderupload (blog wurde gespeichert)");

                        //showAlert("error", data.error, undefined, $('#imageUploadModalMessage'));
                    }else if (data.success){
                        console.log("data.success " + data.success);
                        alert("Blog und Bild gespeichert");
//                        showAlert("success", "Das Bild wurde erfolgreich hochgeladen", undefined, $('#imageUploadModalMessage'));
                    }else{
                        //console.log("else: " + data);
                        alert ("Fehler beim Bilderupload (blog wurde gespeichert)");
                    }

                })

                .error(function (data, status, headers, config) {
                    console.log("error: " + data);
                })
        } else {
            alert("gespeichert");
        }
    };

    $scope.SelectImage=function() {
        if (!$scope.Editable) {
            return;
        }
        $('#uploadImage').trigger('click');
     //   $scope.LoadImage();
    };

    $scope.GetUrl=function() {
        var baseHref = "http://www.bleifood.de/";
        return baseHref+"article/"+$scope.Id;
    };

    $scope.FileSelected=function() {
        var img=$('#uploadImage')[0];
        if (img.files===undefined) {
            return; // Bild wurde nicht verändert
        }
        var f = img.files[0]; // document.getElementById('file').files[0],
        r = new FileReader();
        r.onloadend = function(e){
            //var data = e.target.result;
            $scope.SelectedImageFile= e.target.result;
            $('.blogImage').attr('src', $scope.SelectedImageFile);
        };
        r.readAsDataURL(f);
    };

    $scope.GetDates=function(article) {

        var d=article.dateCreated;
        article.created  = new Date(d.substr(0, 4), d.substr(5, 2)-1, d.substr(8, 2), d.substr(11, 2), d.substr(14, 2), d.substr(17, 2));
        //article.created = new Date(article.dateCreated);
        article.monthCreated = $scope.niceMonth(article.created);
        article.dayCreated = article.created.getDate();
    };
    $scope.GetAllCategories=function(returnFunction) {
        $http.get(apiUrl + 'blog/categories')
            .success(function (data, status, headers, config) {
                $scope.Categories=data;
                if (returnFunction!==undefined) {
                    returnFunction();
                }
            });
    };
    $scope.GetAllAuthors=function() {
        if ($scope.Editable) {
            $http.get(apiUrl + 'blog/authors')
                .success(function (data, status, headers, config) {
                    $scope.Authors = data;
                    $scope.SelectedAuthor = $.grep($scope.Authors, function (e) {
                        return e.id == $scope.Article.authorId;
                    })[0];
                });
        }
    };
    $scope.GetAllTags=function() {
        $http.get(apiUrl + 'blog/tags')
            .success(function (data, status, headers, config) {
                $scope.Tags=data;
            });
    };
    $scope.initTeasers=function() {
        if ($scope.SearchCategory===undefined) {
            $scope.SearchCategory="all";
        }
        if ($scope.SearchTag===undefined) {
            $scope.SearchTag="all";
        }
        if ($scope.Page===undefined || $scope.Page<1) {
            $scope.Page=1;
        }
        $scope.GetAllCategories();
        $scope.GetAllTags();

        var category=$scope.SearchCategory;
        var tag=$scope.SearchTag;

        var teaserUrl=apiUrl + 'blog/teasers/1/0/'+($scope.Page-1);
        var countUrl=apiUrl+"blog/count/1/0";

        if (category!=="all") {
            teaserUrl=apiUrl + 'blog/teasersByCat/1/'+category+'/'+($scope.Page-1);
            countUrl=apiUrl+"blog/countByCat/1/"+category;
        } else if (tag!=="all") {
            teaserUrl=apiUrl + 'blog/teasersByTag/1/'+tag+'/'+($scope.Page-1);
            countUrl=apiUrl+"blog/countByTag/1/"+tag;
        }

        // Keine Einschränkung
        $http.get(countUrl)
            .success(function (data, status, headers, config) {
               $scope.SetPageArray(data);
            });

        $http.get(teaserUrl)
            .success(function (data, status, headers, config) {
                $scope.Teasers = data;
                $scope.Teasers.forEach(function (teaser) {
                    $scope.GetDates(teaser);
                })
            })
    };

    $scope.AssignEdits=function() {
        $scope.Article.content=$('.blogContent')[0].innerText;
        $scope.Article.teaser=$('.blogTeaser')[0].innerText;
        $scope.Article.title=$('.blog_title')[0].innerText;
        $scope.Article.dateCreated=$('.meta_date')[0].innerText;
        $scope.Article.Tags=$scope.ArticleTags;
        $scope.Article.authorName=$scope.SelectedAuthor.name;
        $scope.Article.authorId=$scope.SelectedAuthor.id;
        $scope.Article.categoryId=$scope.SelectedCategory.id;


        var dateVals=$scope.Article.dateCreated.split('.');
        $scope.Article.dateCreated=dateVals[2]+"-"+dateVals[1]+"-"+dateVals[0];

    };

    $scope.SaveEdit=function() {
      $scope.AssignEdits();
        $http.post(apiUrl + 'blog/update', {
            blogEntry: $scope.Article
        })
            .success(function (data, status, headers, config) {
                $scope.UploadImage($scope.Article.id, $scope.Article.imageId);
            })
            .error(function (data, status, headers, config) {
                alert("FEHLER!");
            });
    };

    $scope.SaveNew=function() {
        $scope.AssignEdits();
        $http.post(apiUrl + 'blog/set', {
            blogEntry: $scope.Article
        })
            .success(function (data, status, headers, config) {
                var newId=data;
                $scope.UploadImage(newId,"undefined");

            })
            .error(function (data, status, headers, config) {
                alert("FEHLER!");
            });
    };


});

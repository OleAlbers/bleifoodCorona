/**
 * Created by Tomato on 26.04.2015.
 */


var apiUrl="./api/";

/** Soziale Netzwerke **/

socialNetworks=[
    {
        id:"googleplus",
        url:"https://plus.google.com/111738167839153423392/posts"
    },
    {
        id:"twitter",
        url:"https://twitter.com/bleifood"
    },
    {
        id:"facebook",
        url:"https://www.facebook.com/pages/Bleifoodde/1703288969899035"
    }
];

disqusSupport=3957092;

/**
 * Automatische Fehlermeldung (inline) für API-Access
 * @param status Statuscode
 * @param $parent Parent, Optional. Notwendig bei mehreren Alertboxen
 */
redirectOnError=function(status, $parent, $popup) {
    if ($popup==undefined) {
        $popup=false;}
    switch(status) {
        case 404:
            showAlert("warn","Daten konnten nicht gefunden werden",404, $parent,$popup);
            break;
        case 401:
            window.location="login";
            break;
        case 403:
            showAlert("error","Unzureichende Berechtigungen",403, $parent,$popup);
            break;
        case 500:
            showAlert("error","Serverfehler. Da ist bestimmt Lars schuld... :)",500, $parent,$popup);
            break;
        case "loginSuccessFul":
            window.location="dashboard/help";
            break;
        default:
            showAlert("error","Unerwarteter Fehler",undefined, $parent,$popup);
            break;
    }
};

/**
 * Inline-Meldung anzeigen
 * @param type Typ (warn, success, error)
 * @param content Textinhalt
 * @param status Statuscode (optional, definiert icons)
 * @param $parent Parent
 */
showAlert=function(type, content, status, $parent,  $popup) {
    var iconClass="fa-info-circle";
    var boxClass="alert-info";
    var title="Hinweis: ";

    switch (type) {
        case "warn":
            iconClass="fa-exclamation-circle";
            boxClass="alert-warning";
            title="Warnung: ";
            break;
        case "error":
            iconClass="fa-exclamation-triangle";
            boxClass="alert-danger";
            title="Fehler: ";
            break;
        case "success":
            iconClass="fa-thumbs-o-up";
            boxClass="alert-success";
            title="";
            break;
    }
    if (status!==undefined) {
        // Status-Spezifische Warnungen/Meldungen
        switch(status) {
            case 404:
                // Not found
                iconClass="fa-chain-broken";
                break;
            case 500:
                // Serverfehler
                iconClass="fa-bug";
                break;
            case 401:
                // Session abgelaufen
                iconClass="fa-clock-o";
                break;
            case 403:
                // fehlende Permissions
                iconClass="fa-lock";
                break;
        }
    }
    if ($parent===undefined || $parent===null) {
        $parent=$('body');
    }

    if ($popup) {
        showPopup(type, title, content, null,  iconClass, null, null);
        return;
    }

    var $alert=$parent.find('.alert');
    // Alte Klassen entfernen:
    $alert.removeClass("alert-success");
    $alert.removeClass("alert-warning");
    $alert.removeClass("alert-danger");
    $alert.removeClass("alert-info");
    var $icon=$parent.find('.alert').find('i.fa');
    $icon.removeClass();

    // Neue Klassen setzen:
    $alert.addClass(boxClass);
    $icon.addClass('fa');
    $icon.addClass('fa-2x');
    $icon.addClass(iconClass);

    // Text setzen:
    $alert.find('strong').text(title);
    $alert.find('span.content').text(content);

    $alert.fadeIn();
}

/**
 * Vorhandenen Alert verstecken
 * @param $parent Parent
 */
clearAlert=function($parent) {
    if ($parent===undefined) {
        $parent=$('body');
    }
    $parent.find('.alert').fadeOut();
}

/**
 * Suchparameter ermitteln (OldSchool, ohne Routing)
 * @returns {{}} Array der Query
 */
var searchQuery = function() {
    var str = window.location.search;
    var objURL = {};

    str.replace(
        new RegExp( "([^?=&]+)(=([^&]*))?", "g" ),
        function( $0, $1, $2, $3 ){
            objURL[ $1 ] = $3;
        }
    );
    return objURL;
};

/**
 * Suchparameter bestimmen
 * @param key Paramtername
 * @returns {*} Parameterwert
 */
var search=function(key) {
    if ($('[name="urlQuery"]').length>0) {
        var query= $.parseJSON($('[name="urlQuery"]').val());
    } else {
        var query = searchQuery();
    }
    if (query===undefined || query===null){
        return undefined;
    }
    return query[key];
};


/**
 * Universelles Popup anzeigen
 * @param type Typ ("delete", "message","nopermission","fatal","custom")
 * @param title Titel
 * @param text Mitteilung
 * @param buttons Optional, wenn custom Type ("YesNo" oder "Ok)
 * @param icon Optional, wenn custom Type
 * @param successAction Methode, die beim "OK" oder "Ja"-Klick ausgeführt werden soll
 * @param failAction Methode, die beim "Nein"-Klick ausgeführt werden soll
 */
var showPopup=function(type, title, text, buttons,  icon, successAction, failAction) {
    // Icons und Buttons anhand des Typs setzen:
    if (buttons===undefined || buttons===null) {
        switch (type) {
            case "delete":
                buttons="YesNo";
                break;
            case "message":
            case "warn":
            case "error":
                buttons="Ok";
                break;
            case "nopermission":
                buttons="Ok";
                break;
            case "fatal":
                buttons="Ok";
                break;
        }
    }
    if (icon===undefined || icon===null) {
        switch (type) {
            case "delete":
                icon="fa-bomb";
                break;
            case "message":
                icon="fa-info-circle";
                break;
            case "nopermission":
                icon="fa-lock";
                break;
            case "fatal":
                icon="fa-bug";
                break;
        }
    }

    // Box und Buttons finden:
    var box=$('#messageBox');
    var okButton=box.find('.dialogOk');
    var yesButton=box.find('.dialogYes');
    var noButton=box.find('.dialogNo');

    // Events zuweisen:
    okButton.data("event",successAction);
    yesButton.data("event",successAction);
    noButton.data("event",failAction);

    // Inhalte zuweisen:
    switch (buttons) {
        case "Ok":
            okButton.show();
            yesButton.hide();
            noButton.hide();
            break;
        case "YesNo":
            okButton.hide();
            yesButton.show();
            noButton.show();
            break;
    }
    box.find('h4').text(title);
    box.find('p').html(text);
    box.find('.icon i').removeClass();
    box.find('.icon i').addClass("fa");
    box.find('.icon i').addClass("fa-realfat");
    box.find('.icon i').addClass(icon);

    // Zentrieren:
    box.css("marginLeft","-"+Math.floor(box.width()/2)+"px");
    box.css("marginTop","-"+Math.floor(box.height()/2)+"px");

    // Icon entfernen bei kleiner Auflösung:
    if ($(window).width()<990) {
        // kleines Device, Icon wech:
        box.find('.icon').hide();
        $('#messageBox').css('min-width','');
    } else {
        $('#messageBox').css('min-width','800px');
        box.find('.icon').show();
    }


    // Anzeigen:
    $('.overlay').fadeIn();
    box.fadeIn();
};

/**
 * "close"-Element geklickt
 */
$(document).on('click','.close',function() {
    var hideControl=$(this).data('hide');
    if (hideControl!==undefined && hideControl.length>0) {
        $(this).closest('.'+hideControl).fadeOut();
    }
});

/**
 * "minimizeDiv"-Element geklickt
 */
$(document).on('click','.minimizeDiv',function(){
    $(this).parent().find('.row').slideUp();
    //$(this).parent().find('p').hide();

    $(this).removeClass('minimizeDiv');
    $(this).removeClass('fa-chevron-up');
    $(this).addClass('maximizeDiv');
    $(this).addClass('fa-chevron-down');
});

/**
 * "maximizeDiv"-Element geklickt
 */
$(document).on('click','.maximizeDiv',function(){
    $(this).parent().find('.row').slideDown();
    $(this).removeClass('maximizeDiv');
    $(this).removeClass('fa-chevron-down');
    $(this).addClass('minimizeDiv');
    $(this).addClass('fa-chevron-up');
});

/**
 * beliebigen Button bei MessageBox geklickt
 */
$(document).on('click','.dialogButtons .btn',function() {
    $('.overlay').fadeOut();
    $('#messageBox').fadeOut();
    if ($(this).data("event")!==undefined && $(this).data("event")!==null) {
        $(this).data("event")();
    }
});

/**
 * Prüfen, ob ein Element einen Wert enthält
 * @param str
 * @returns {boolean}
 */
function IsNullOrWhiteSpace(str){
    return str === null || str.match(/^\s*$/) !== null;
}




var getAddr = function(addr, f){

    var geocode = new Object();
    var geocoder = new google.maps.Geocoder();

    if(typeof addr != 'undefined' && addr != null) {
        geocoder.geocode( { address: addr }, function(results, status) {
            if (status == google.maps.GeocoderStatus.OK) {

                console.log("lat and long for address: " + addr);
                console.log(results[0].geometry.location);

                console.log("lat: " + results[0].geometry.location.lat());
                console.log("lng: " + results[0].geometry.location.lng());

                geocode.latitude = results[0].geometry.location.lat();
                geocode.longitude = results[0].geometry.location.lng();

                f('ok', geocode);
            } else {
                f('error', null);
            }
        });
    } else {
        f('error', null);
    }
};

var searchModal=function(key) {
    if ($('[name="modalQuery"]').length>0) {
        var query= $.parseJSON($('[name="modalQuery"]').val());
    } else {
        var query = searchQuery();
    }
    if (query===undefined || query===null){
        return undefined;
    }
    return query[key];
};

var changeBackground=function(back) {
    var imageUrl="./html/assets/flavours/ribsndogs/images/content/"+back;
    $('.maincontent').css('background-image', 'url(' + imageUrl + ')');
};
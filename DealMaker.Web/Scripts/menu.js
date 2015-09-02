function loadPageOnFrame( url) {
    var $iframe = $('iframe#Iframe');
    if ($iframe.length) {
        $iframe.attr('src', url);
        return false;
    }
    return true;
}
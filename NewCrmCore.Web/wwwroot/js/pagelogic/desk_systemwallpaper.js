﻿NewCrm.Desk.SystemWallpaper = {
    wallpaperType: ''
};

$("#wallpapertype").val(NewCrm.Desk.SystemWallpaper.wallpaperType);

$("#wallpapertype").on('change', () => {
    top.HROS.wallpaper.update($('#wallpapertype').val(), '');
});

$('.wallpaper li').on('click', function () {
    top.HROS.wallpaper.update($('#wallpapertype').val(), $(this).attr('wpid'));
});
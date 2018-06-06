$('#add_more').click(function () {
    "use strict";
    $(this).before($("<div/>", {
        id: 'filediv'
    }).fadeIn('slow').append(
      $("<input/>", {
          name: 'file[]',
          type: 'file',
          id: 'file',
          multiple: 'multiple',
          accept: 'image/*'
      })
    ));
});

$('#upload').click(function (e) {
    "use strict";
    e.preventDefault();

    if (window.filesToUpload.length === 0 || typeof window.filesToUpload === "undefined") {
        alert("No files are selected.");
        return false;
    }

    // Now, upload the files below...
    // https://developer.mozilla.org/en-US/docs/Using_files_from_web_applications#Handling_the_upload_process_for_a_file.2C_asynchronously
});

deletePreview = function (ele, i) {
    "use strict";
    try {
        $(ele).parent().remove();
        window.filesToUpload.splice(i, 1);
    } catch (e) {
        console.log(e.message);
    }
}

$("#file").on('change', function () {
    "use strict";

    // create an empty array for the files to reside.
    window.filesToUpload = [];

    if (this.files.length >= 1) {
        $("[id^=previewImg]").remove();
        $.each(this.files, function (i, img) {
            var reader = new FileReader(),
              newElement = $("<div id='previewImg" + i + "' class='previewBox'><img /></div>"),
              deleteBtn = $("<span class='delete' onClick='deletePreview(this, " + i + ")'>X</span>").prependTo(newElement),
              preview = newElement.find("img");

            reader.onloadend = function () {
                preview.attr("src", reader.result);
                preview.attr("alt", img.name);
            };

            try {
                window.filesToUpload.push(document.getElementById("file").files[i]);
            } catch (e) {
                console.log(e.message);
            }

            if (img) {
                reader.readAsDataURL(img);
            } else {
                preview.src = "";
            }

            newElement.appendTo("#filediv");
        });
    }
});
function preview_images() {
    var total_file = document.getElementById("images").files.length;
    for (var i = 0; i < total_file; i++) {
        $('#image_preview').append("<img class='img-responsive' src='" + URL.createObjectURL(event.target.files[i]) + "'>");
    }
    ResizeImage();
};

$(window).resize(ResizeImage);
$(window).on("load", ResizeImage);

$('#logoff').on("click", function (e) {
    e.preventDefault();
    $.ajax({
        url: '/User/Logoff',
    });
    window.location.reload();
});


function ResizeImage(e) {
    var w = window.innerWidth;
    if (w <= 500) {
        $('.img-responsive').removeClass("tabletImage");
        $('.img-responsive').removeClass("webImage");
        $('.img-responsive').removeClass("miniTabletImage");
        $('.img-responsive').addClass("mobileImage");
        $('.image_preview_custom').removeClass("ImageRowSmall");
        $('.image_preview_custom').removeClass("ImageRowBig");
        $('.image_preview_custom').addClass("ImageRowSmall");
    }
    else if (w >= 501 && w <= 620) {
        $('.img-responsive').removeClass("tabletImage");
        $('.img-responsive').removeClass("webImage");
        $('.img-responsive').removeClass("mobileImage");
        $('.img-responsive').addClass("miniTabletImage");
        $('.image_preview_custom').removeClass("ImageRowSmall");
        $('.image_preview_custom').removeClass("ImageRowBig");
        $('.image_preview_custom').addClass("ImageRowSmall");
    }
    else if (w >= 621 && w <= 980) {
        $('.img-responsive').removeClass("webImage");
        $('.img-responsive').removeClass("mobileImage");
        $('.img-responsive').removeClass("miniTabletImage");
        $('.img-responsive').addClass("tabletImage");
        $('.image_preview_custom').removeClass("ImageRowSmall");
        $('.image_preview_custom').removeClass("ImageRowBig");
        $('.image_preview_custom').addClass("ImageRowBig");
    }
    else {
        $('.img-responsive').removeClass("tabletImage");
        $('.img-responsive').removeClass("mobileImage");
        $('.img-responsive').removeClass("miniTabletImage");
        $('.img-responsive').addClass("webImage");
        $('.image_preview_custom').removeClass("ImageRowSmall");
        $('.image_preview_custom').removeClass("ImageRowBig");
        $('.image_preview_custom').addClass("ImageRowBig");
    }
}
$(window).on("load", function (e) {
    var paddingList = $('.myPadding');
    for (var i = 0; i < paddingList.length; i++) {
        $(($(paddingList[i]).children())[0]).attr("style", "margin-left:15px")
    } });


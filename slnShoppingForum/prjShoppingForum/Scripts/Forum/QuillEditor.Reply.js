//回復文章的區塊，只能讀取，不能修改，設定readonly，且工具列是null
var quill = new Quill('#editor', {
    modules: {
        toolbar: null
    },
    readOnly: true,
    theme: 'snow'
});

var tmphtml = $(".targetSource").html();
$(".ql-editor").html(tmphtml.trim());    //移除會自動產生的多餘<p>段落 - 頭尾


//回覆回覆的區塊
var toolbarOptions = [
    ['bold', 'italic', 'underline'],        // toggled buttons
    ['blockquote', 'code-block'],
    [{ 'script': 'super' }, { 'script': 'sub' }],      // superscript/subscript
    [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
    ['image', 'video'],
    ['clean']                                         // remove formatting button
];
var quill = new Quill('#commentDiv', {
    modules: {
        toolbar: toolbarOptions
    },
    theme: 'snow',
    placeholder: '請輸入要回覆的內容'
});
var quill2 = new Quill('#commentforcommentDiv', {
    modules: {
        toolbar: toolbarOptions
    },
    theme: 'snow',
    placeholder: '請輸入要回覆的內容'
});

//註冊事件 - 當失去焦點，要可以抓到回覆區塊內的資料
//回復文章
$(".ql-editor").eq(1).blur(function () {
    let content = $(".ql-editor").eq(1).html();
    $("#tmpContent").text(content);
});

//回覆回覆
$(".ql-editor").eq(2).blur(function () {
    let content = $(".ql-editor").eq(2).html();
    $("#tmpContent1").text(content);
});

//回覆回覆要點選後才會顯示
$(".btn-comment").click(function () {
    $("#tmpReplyType1").text("COMMENT");
    $("#tmpTargetId1").text($(this).parent("div").attr("data-id"));
    $("#commentforcomment").css("display","block")
})

//階層越高的，寬度越窄
let commentList = $(".commentElement");
for (var i = 0; i < commentList.length; i++) {
    let targ = commentList.eq(i);
    targ.css("width", (600 - parseInt(targ.attr("data-seq")) * 50).toString() + "px");
}

//注意!! EventHandle.Edit 有修改，這邊也要做確認，避免兩邊IDmapping錯誤

//建立Quill文章編輯器
var toolbarOptions = [
    ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
    ['blockquote', 'code-block'],
    [{ 'list': 'bullet' }, { 'list': 'ordered' }],
    [{ 'script': 'super' }, { 'script': 'sub' }],      // superscript/subscript
    [{ 'direction': 'rtl' }],                         // text direction
    [{ 'indent': '+1' }, { 'indent': '-1' }],          // outdent/indent
    [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
    [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
    [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
    [{ 'font': [] }],
    [{ 'align': [] }],
    ['image', 'video'],
    ['clean']                                         // remove formatting button
];

//ID選擇器：#editor <- 注意!
var quill = new Quill('#editor', {
    modules: {
        toolbar: toolbarOptions
    },
    theme: 'snow'
});

//在離開編輯區塊時，把資料
//ID選擇器：#tmpContent <- 注意
$(".ql-editor").blur(function () {
    let content = $(".ql-editor").html();
    $("#tmpContent").text(content);
})

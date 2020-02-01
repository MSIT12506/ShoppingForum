//注意!! QuillEditor.New_Update.Post.js 有修改，這邊也要做確認，避免兩邊IDmapping錯誤

var tmphtml = $(".targetSource").html();
$(".ql-editor").html(tmphtml.trim());           //移除會自動產生的多餘<p>段落 - 頭尾(多餘的空格導致瀏覽器會額外新增頭尾兩組 <p></p> )
$("#tmpContent").text($(".ql-editor").html());  //ini 頁面一打開就先給值，避免null導致error
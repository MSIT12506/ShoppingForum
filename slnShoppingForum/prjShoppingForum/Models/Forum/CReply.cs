using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tw.com.essentialoil.Forum.ViewModels;

namespace tw.com.essentialoil.Forum.Models
{
    public class CReply
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();
        //Create New Reply
        public void NewCommentForPost(CNewReplyCreate vm)
        {
            tForumReply reply = new tForumReply();
            reply.fPostId = vm.tmpPostId;
            reply.fReplyId = Guid.NewGuid().ToString();
            reply.fReplyTargetId = vm.tmpTargetId;
            reply.fReplySeqNo = 0;
            reply.fUserId = "user";   //TODO - 要動態產生
            reply.fReplyTime = DateTime.Now;
            reply.fEnableFlag = true;
            reply.fContent = vm.tmpContent;

            db.tForumReplies.Add(reply);
            db.SaveChanges();
        }

        //Create New Reply For Comment
        public void NewCommentForComment(CNewReplyCreate vm)
        {
            tForumReply targetReply = (from i in db.tForumReplies
                                       where i.fReplyId == vm.tmpTargetId && i.fEnableFlag == true
                                       select i).FirstOrDefault();

            if (targetReply != null)
            {
                tForumReply reply = new tForumReply();
                reply.fPostId = vm.tmpPostId;
                reply.fReplyId = Guid.NewGuid().ToString();
                reply.fReplyTargetId = vm.tmpTargetId;
                reply.fReplySeqNo = targetReply.fReplySeqNo + 1;
                reply.fUserId = "user";   //TODO - 要動態產生
                reply.fReplyTime = DateTime.Now;
                reply.fEnableFlag = true;
                reply.fContent = vm.tmpContent;

                db.tForumReplies.Add(reply);
                db.SaveChanges();
            }


        }


        //Get All Reply By Id
        public List<List<tForumReply>> getReplysById(int fPostId)
        {
            List<List<tForumReply>> result = new List<List<tForumReply>>();

            //先取得該文章的所有留言
            var allReply = from m in db.tForumReplies
                           where m.fPostId == fPostId
                           select m;

            List<tForumReply> allReplyList = allReply.ToList();

            //分別取得每一個階層的留言清單，並依照建立時間排序
            //回傳結果
            //[[第一則留言的所有階層],[第二則留言的所有階層],[第三則留言的所有階層],...]
            int startNum = 0;  //階層
            if (allReplyList.Count > 0)
            {
                List<tForumReply> m_num = (from i in allReplyList
                                           where i.fReplySeqNo == startNum
                                           orderby i.fReplyTime ascending
                                           select i).ToList();

                
                for (int i = 0; i < m_num.Count; i++)
                {
                    List<tForumReply> tmpReply = new List<tForumReply>();
                    getCommentFlow(m_num[i], tmpReply);
                    result.Add(tmpReply);
                }
            }

            return result;
        }

        public void getCommentFlow(tForumReply reply, List<tForumReply> result)
        {
            result.Add(reply);
            string targetId = reply.fReplyId;
            var lvNum = (from i in db.tForumReplies
                      where i.fReplyTargetId == targetId
                      orderby i.fReplySeqNo ascending, i.fReplyTime ascending
                      select i).ToList();

            if (lvNum.Count != 0)
            {
                for (int i = 0; i < lvNum.Count; i++)
                {
                    getCommentFlow(lvNum[i], result);
                }
            }
            
        }
    }
}
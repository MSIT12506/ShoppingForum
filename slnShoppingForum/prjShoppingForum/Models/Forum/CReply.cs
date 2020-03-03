﻿using prjShoppingForum.Models.Entity;
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
        public void NewCommentForPost(CNewReplyCreate replyInfo, int userId)
        {
            tForumReply reply = new tForumReply();
            reply.fPostId = replyInfo.postId;
            reply.fReplyId = Guid.NewGuid().ToString();
            reply.fReplyTargetId = replyInfo.targetId;
            reply.fReplySeqNo = 0;
            reply.fId = userId;
            reply.fReplyTime = DateTime.UtcNow.AddHours(8);
            reply.fEnableFlag = true;
            reply.fContent = replyInfo.content;

            db.tForumReplies.Add(reply);
            db.SaveChanges();
        }

        //Create New Reply For Comment
        public void NewCommentForComment(CNewReplyCreate replyInfo, int userId)
        {
            tForumReply targetReply = (from i in db.tForumReplies
                                       where i.fReplyId == replyInfo.targetId && i.fEnableFlag == true
                                       select i).FirstOrDefault();

            if (targetReply != null)
            {
                tForumReply reply = new tForumReply();
                reply.fPostId = replyInfo.postId;
                reply.fReplyId = Guid.NewGuid().ToString();
                reply.fReplyTargetId = replyInfo.targetId;
                reply.fReplySeqNo = targetReply.fReplySeqNo + 1;
                reply.fId = userId;
                reply.fReplyTime = DateTime.UtcNow.AddHours(8);
                reply.fEnableFlag = true;
                reply.fContent = replyInfo.content;

                db.tForumReplies.Add(reply);
                db.SaveChanges();
            }
        }


        //Get All Reply By Id
        public List<List<CPostReplyInfo>> getReplysById(int fPostId)
        {
            List<List<tForumReply>> result = new List<List<tForumReply>>();
            List<List<CPostReplyInfo>> results = new List<List<CPostReplyInfo>>();

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

            foreach (var item in result)
            {
                List<CPostReplyInfo> replyInfoList = new List<CPostReplyInfo>();

                foreach (var reply in item)
                {
                    CPostReplyInfo info = new CPostReplyInfo() { replyId = reply.fReplyId, replyContent = reply.fContent, replyTime = reply.fReplyTime, replySeqNo = reply.fReplySeqNo, userName = reply.tUserProfile.fName, isLikeOrHate = null };
                    bool? q = db.tForumReplyAnalysis.Where(r => r.fId == reply.fId && r.fPostId == reply.fPostId && r.fReplyId == reply.fReplyId).Select(r => r.fLikeHate).FirstOrDefault();

                    if (q != null)
                    {
                        info.isLikeOrHate = q.ToString();
                    }

                    replyInfoList.Add(info);
                }

                results.Add(replyInfoList);
            }

            return results;
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

        //Get All New Reply By Time
        public List<tForumReply> getNewReplysByTime(int postId, DateTime targetTime) {

            var replys = from m in db.tForumReplies
                         where m.fEnableFlag == true && m.fPostId == postId && m.fReplyTime > targetTime
                         orderby m.fReplyTime
                         select m;

            return replys.ToList();
        }

        //新增留言的【喜歡/討厭】紀錄
        public void addLikeOrHateCount(int userId, int postId, string replyId, bool result)
        {
            //檢查是否已經有討厭或是喜歡的紀錄
            tForumReplyAnalysi checkRecord = db.tForumReplyAnalysis.Where(p => p.fId == userId && p.fPostId == postId && p.fReplyId == replyId).FirstOrDefault();

            if (checkRecord == null)
            {
                tForumReplyAnalysi addLikeRecord = new tForumReplyAnalysi();
                addLikeRecord.fId = userId;
                addLikeRecord.fPostId = postId;
                addLikeRecord.fReplyId = replyId;
                addLikeRecord.fLikeHate = result;
                db.tForumReplyAnalysis.Add(addLikeRecord);
                db.SaveChanges();
            }
            else
            {
                checkRecord.fLikeHate = result;
                db.SaveChanges();
            }
        }
    }
}
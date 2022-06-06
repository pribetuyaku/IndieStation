using System;

namespace AppFinal.Models
{
    /// <summary>
    /// Message Status Enumerator
    /// </summary>
    public enum MessageStatus
    {
        SENT,
        DELIVERED,
        RECEIVED,
        READ,
        LIKED
            
    }

    /// <summary>
    /// Getter for message status
    /// </summary>
    public static class MessageStatusGetter
    {
        /// <summary>
        /// Get MessageStatus from string
        /// </summary>
        /// <param name="status">string status</param>
        /// <returns>MessageStatus</returns>
        public static MessageStatus GetMessageStatus(string status)
        {
            if (status == null) return MessageStatus.SENT;

            switch (status.ToLower())
            {
                case "sent":
                    return MessageStatus.SENT;
                case "delivered":
                    return MessageStatus.DELIVERED;
                case "received":
                    return MessageStatus.RECEIVED;
                case "read":
                    return MessageStatus.READ;
                case "liked":
                    return MessageStatus.LIKED;
                default:
                    Console.WriteLine("Invalid status string");
                    return MessageStatus.SENT;
            }
        }

        /// <summary>
        /// Get string from MessageStatus
        /// </summary>
        /// <param name="status">MessageStatus</param>
        /// <returns>status string</returns>
        public static string GetMessageStatus(MessageStatus status)
        {
            switch (status)
            {
                case MessageStatus.SENT:
                    return "SENT";
                case MessageStatus.DELIVERED:
                    return "DELIVERED";
                case MessageStatus.RECEIVED:
                    return "RECEIVED";
                case MessageStatus.READ:
                    return "READ";
                case MessageStatus.LIKED:
                    return "LIKED";
                default:
                    Console.WriteLine("Invalid status");
                    return "SENT";
            }
        }
    }
}
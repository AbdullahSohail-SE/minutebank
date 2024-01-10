using minutebank.Models;
using System.Data.SqlClient;

namespace minutebank.Mapper
{
    public class DBMapper
    {
        public static Func<SqlDataReader, User> userMapper = (SqlDataReader reader) =>
        {
            return new User()
            {
                name = (string)reader["name"],
                email = (string)reader["email"],
                password = (string)reader["password"],
                id = (int)reader["id"]
            };
        };

        public static Func<SqlDataReader, Recipient> recipientMapper = (SqlDataReader reader) =>
        {
            return new Recipient()
            {
                id = (int)reader["id"],
                name = (string)reader["name"],
                bank = (string)reader["bank"],
                swift_code = (short)reader["swift_code"],
                account_number = (string)reader["account_number"],
                user_id = (int)reader["user_id"],
            };
        };


        public static Func<SqlDataReader, PaymentRequest> paymentRequestMapper = (SqlDataReader reader) =>
        {
            return new PaymentRequest()
            {
                id = (int)reader["id"],
                sent_to = (string)reader["sent_to"],
                amount = (long)reader["amount"],
                due_by = (DateTime)reader["due_by"],
                status = (bool)reader["status"],
                account_number = (string)reader["account_number"],
                user_id = (int)reader["user_id"],
            };
        };

        public static Func<SqlDataReader, Credit> creditMapper = (SqlDataReader reader) =>
        {
            return new Credit()
            {
                id = (int)reader["id"],
                date = (DateTime)reader["date"],
                amount = (long)reader["amount"],
                from_account_number = reader["from_account_number"] != DBNull.Value
                ? (string)reader["from_account_number"]
                : null,
                payment_request_id = reader["payment_request_id"] != DBNull.Value
                ? (int)reader["payment_request_id"]
                : null,
                account_id = (int)reader["account_id"]
            };
        };

        public static Func<SqlDataReader, Debit> debitMapper = (SqlDataReader reader) =>
        {
            return new Debit()
            {
                id = (int)reader["id"],
                date = (DateTime)reader["date"],
                amount = (long)reader["amount"],
                to_account_number = reader["to_account_number"] != DBNull.Value
                ? (string)reader["to_account_number"]
                : null,
                recipient_id = reader["recipient_id"] != DBNull.Value
                ? (int)reader["recipient_id"]
                : null,
                account_id = (int)reader["account_id"]
            };
        };

        public static Func<SqlDataReader, Card> cardMapper = (SqlDataReader reader) =>
        {
            return new Card()
            {
                id = (int)reader["id"],
                card_number = (string)reader["card_number"],
                expiry = (DateTime)reader["expiry"],
                cvc = (string)reader["cvc"],
                account_id = (int)reader["account_id"],
            };
        };

        public static Func<SqlDataReader, Account> accountMapper = (SqlDataReader reader) =>
        {
            return new Account()
            {
                id = (int)reader["id"],
                account_number = (string)reader["account_number"],
                balance = (long)reader["balance"],
                type = (string)reader["type"],
                status = (bool)reader["status"],
                user_id = (int)reader["user_id"],
            };
        };

        public static Func<SqlDataReader, AccountStats> accountStatsMapper = (SqlDataReader reader) =>
        {
            return new AccountStats()
            {
                type = (string)reader["type"],
                date = (DateTime)reader["date"],
                account = (string)reader["account"],
                amount = (long)reader["amount"],
                name = (string)reader["name"],
                counterparty_account = (string)reader["counterparty_account"],

            };
        };

        public static Func<SqlDataReader, Summary> summaryMapper = (SqlDataReader reader) =>
        {
            return new Summary()
            {
                balance = reader["balance"] != DBNull.Value ? (long)reader["balance"] : 0,
                credit = reader["credit"] != DBNull.Value ? (long)reader["credit"] : 0,
                debit = reader["debit"] != DBNull.Value ? (long)reader["debit"] : 0,
            };
        };
    }
}

namespace ScannerHelper
{
    public enum Tok
    {
        EOF = 0, ID, INUM, RNUM, COLON, SEMICOLON, ASSIGN, BEGIN, END, CYCLE,
        STRING, COMMENT, COMMENT_BEGIN, COMMENT_END, ID2
    };
}
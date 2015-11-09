lexer grammar Tokenizer;

STAR        : '*' ;
SLASH       : '/' ;
WHITESPACE  : [ \r\n\t]+ ;
IDENT       : ('a'..'z'|'A'..'Z'|'0'..'9')+ ;
COMMENT     : '/*' (COMMENT | .)*? ('*/' | '\n'| '\r\n' | EOF) ;
LIT_STR     : '"' ('\\\n' | '\\\r\n' | .)*? ('"' | EOF) ;
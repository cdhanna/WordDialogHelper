grammar WordLang;

/*
 * Parser Rules
 */
block: prog;
prog: rule+ EOF;

rule: WORD;
/*
 * Lexer Rules
 */

fragment UPPER: ('A'..'Z')+;
fragment LOWER: ('a'..'z')+;

WORD : (UPPER|LOWER)+;

NL	: ('\r'?'\n' | '\r')+;
WS	: (' '|'\t')+ -> skip ;
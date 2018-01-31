grammar WordLang;


/*
 * Parser Rules
 */
prog: rule+ EOF;

rule
	: ruleTitle displayAs conditions
	;

ruleTitle
	: TEXT NEWLINE
	;

displayAs
	: DISPLAYAS NEWLINE TEXT NEWLINE
	;

conditions	
	: CONDITIONS NEWLINE singleCondition
	;

singleCondition
	: TEXT EQUALTO NEWLINE
	;

/*
 * Lexer Rules
 */


fragment D : ('D'|'d');
fragment I : ('I'|'i');
fragment S : ('S'|'s');
fragment P : ('P'|'p');
fragment L : ('L'|'l');
fragment A : ('A'|'a');
fragment Y : ('Y'|'y');

fragment C : ('C'|'c');
fragment O : ('O'|'o');
fragment N : ('N'|'n');
fragment T : ('T'|'t');


DISPLAYAS	: D I S P L A Y   A S SPACE*;
CONDITIONS	: C O N D I T I O N S SPACE*; 


fragment SPACE		: ' ';
fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;

EQUALTO		: I S;
REF
	: TEXT*? EQUALTO
	;
TEXT 
	: (LOWERCASE | UPPERCASE | SPACE)+ 
	;

WHITESPACE          : (' '|'\t')+ -> skip ;
NEWLINE             : ('\r'? '\n' | '\r')+ ;


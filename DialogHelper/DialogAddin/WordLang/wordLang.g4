grammar WordLang;

/*

SampleRule
conditions
x is y
b is c

*/

/*
 * Parser Rules
 */
prog: rule+ EOF;

rule
	: ruleTitle displayAs conditions dialogs outcomes
	;

ruleTitle
	: text NEWLINE
	;

displayAs
	: DISPLAYAS (NEWLINE text)? NEWLINE
	;

conditions	
	: CONDITIONS NEWLINE (booleanExpr NEWLINE)*
	;

dialogs
	: DIALOGS NEWLINE dialogLine*
	;

outcomes
	: OUTCOMES NEWLINE (singleOutcome|NEWLINE)* NEWLINE?
	;

singleOutcome
	: text
	;

dialogLine
	: COLON text NEWLINE multilineText NEWLINE
	;

dialogLineSpeaker
	: COLON NAME
	;

singleCondition
	: booleanExpr NEWLINE
	;

booleanExpr
	: text (booleanOp WHITESPACE? text)
	;

booleanOp
	: booleanOpMain NEGATION | NEGATION booleanOpMain | booleanOpMain
	; 

booleanOpMain
	: EQUALTO | GREATERTHAN | LESSTHAN
	;

text
	: (WHITESPACE? NAME WHITESPACE?)+
	;
multilineText
	: (NAME | WHITESPACE | NEWLINE | EQUALTO | NEGATION)*
	;

/*
 * Lexer Rules
 */

COLON : ':';
DISPLAYAS	: D I S P L A Y   A S   SPACE*;
CONDITIONS	: C O N D I T I O N S   SPACE*; 
DIALOGS		: D I A L O G S         SPACE*;
OUTCOMES	: O U T C O M E S       SPACE*;
WHITESPACE          
	: (' '|'\t')+ ;

GREATERTHAN
	: '>';
LESSTHAN
	: '<';

NEGATION
	: WHITESPACE? ((N O T)|'!') WHITESPACE?;
EQUALTO
	: WHITESPACE? ((I S)|'=') WHITESPACE?;

NAME
	: [a-zA-Z_] [a-zA-Z0-9_]*;


SPACE
	: ' ';
LOWERCASE 
	: [a-z];
UPPERCASE 
	: [A-Z];

NEWLINE	
	: WHITESPACE* ('\r'? '\n' | '\r')+ ( ('\r'? '\n' | '\r')+ | WHITESPACE)*;

fragment A : ('A'|'a');
fragment C : ('C'|'c');
fragment D : ('D'|'d');
fragment E : ('E'|'e');
fragment G : ('G'|'g');
fragment I : ('I'|'i');
fragment L : ('L'|'l');
fragment M : ('M'|'m');
fragment N : ('N'|'n');
fragment O : ('O'|'o');
fragment P : ('P'|'p');
fragment S : ('S'|'s');
fragment T : ('T'|'t');
fragment U : ('U'|'u');
fragment Y : ('Y'|'y');


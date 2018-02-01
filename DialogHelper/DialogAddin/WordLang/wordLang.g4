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
	: ruleTitle displayAs conditions
	;

ruleTitle
	: expr NEWLINE
	;

displayAs
	: DISPLAYAS NEWLINE expr NEWLINE
	;

conditions	
	: CONDITIONS NEWLINE singleCondition*
	;

singleCondition
	: comparison NEWLINE
	;

comparison
	: expr (comparisonOp WHITESPACE expr)
	//: expr (expr SPACE?)? ( comparisonOp  expr*)
	;

comparisonOp
	: EQUALTO
	;

expr: (NAME WHITESPACE?)+;

	//;
// prompt sender IS ana
/*
 * Lexer Rules
 */



COLON : ':';

DISPLAYAS	: D I S P L A Y   A S SPACE*;
CONDITIONS	: C O N D I T I O N S SPACE*; 

WHITESPACE          : (' '|'\t')+ ;


EQUALTO		: 'is'|'=';

NAME
	: [a-zA-Z_] [a-zA-Z0-9_]*
	;

SPACE		: ' ';
LOWERCASE  : [a-z] ;
UPPERCASE  : [A-Z] ;


//TEXT 
//	: (LOWERCASE | UPPERCASE | SPACE)+ 
//	;

NEWLINE             : ('\r'? '\n' | '\r')+ ; 
D : ('D'|'d');
I : ('I'|'i');
S : ('S'|'s');
P : ('P'|'p');
L : ('L'|'l');
A : ('A'|'a');
Y : ('Y'|'y');

C : ('C'|'c');
O : ('O'|'o');
N : ('N'|'n');
T : ('T'|'t');
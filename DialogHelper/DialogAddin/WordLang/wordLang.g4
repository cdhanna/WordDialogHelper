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
	: CONDITIONS NEWLINE (booleanExpr NEWLINE)+
	;

dialogs
	: DIALOGS NEWLINE dialogLine*
	;

outcomes
	: OUTCOMES NEWLINE (singleOutcome|NEWLINE)+ NEWLINE?
	;

singleOutcome
	: outcomeFunction
	| outcomeSetter
	;

outcomeSetter
	: (SET referance TO expression)
	| (referance EQUALTO expression)
	;

outcomeFunction
	: (RUN referance)
	| (RUN referance outcomeFunctionNamedBindingList)
	;

outcomeFunctionNamedBindingList
	: (NEWLINE? outcomeFunctionNamedBinding NEWLINE?)+
	;

outcomeFunctionNamedBinding
	: (WITH referance AS expression)
	;

dialogLine
	: COLON text NEWLINE multilineText NEWLINE
	;

dialogLineSpeaker
	: COLON NAME
	;

booleanExpr
	: expression (booleanOp WHITESPACE? expression)
	;

booleanOp
	: booleanOpMain NEGATION | NEGATION booleanOpMain | booleanOpMain
	; 

booleanOpMain
	: EQUALTO | GREATERTHAN | LESSTHAN
	;


expression
	: 
	| additiveExpr
	;

additiveExpr
	: 
	| (multiplicitiveExpr)
	| (multiplicitiveExpr additiveOp additiveExpr)
	;
additiveOp
	: MINUS | PLUS
	;

multiplicitiveExpr
	:
	| (parenableExpr)
	| (parenableExpr multiplicitiveOp multiplicitiveExpr)
	;
multiplicitiveOp
	: MULTIPLY | DIVIDE
	;

parenableExpr
	:
	| (WHITESPACE? term WHITESPACE?)
	| (LEFT_PAREN expression RIGHT_PAREN)
	;

term
	: (referance | literal)
	;
	//x * y + z * w * a + 1

numberExpression
	: WHITESPACE? (PLUS|MINUS)?(referance | numLiteral) WHITESPACE?
	;

referance
	: NAME | NAME (referanceSeparator referance)?
	;
referanceSeparator
	: WHITESPACE | DOT
	;

literal
	: boolLiteral
	| stringLiteral
	| numLiteral
	;

boolLiteral
	: TRUE | FALSE
	;

stringLiteral
	: (QUOTE (text|WHITESPACE?) QUOTE)
	| (DBLQUOTE (text|WHITESPACE?) DBLQUOTE)
	;

numLiteral
	: INTEGER
	;

text
	: (WHITESPACE? (NAME|INTEGER) WHITESPACE?)+
	;
multilineText
	: (NAME | WHITESPACE | NEWLINE | reservedWord)*
	;
reservedWord
	: EQUALTO
	| NEGATION
	| TRUE
	| FALSE
	| DOT
	| SET
	| TO
	| DISPLAYAS
	| CONDITIONS
	| DIALOGS
	| OUTCOMES
	| MODIFY
	| BY
	| MINUS
	| PLUS
	| MULTIPLY
	| DIVIDE
	| COMMA
	| WITH
	| RUN
	| AS
	;

/*
 * Lexer Rules
 */

COLON : ':';
COMMA: ',';
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


RUN
	: WHITESPACE? (R U N) WHITESPACE?;
WITH
	: WHITESPACE? (W I T H) WHITESPACE?;
AS
	: WHITESPACE? (A S) WHITESPACE?;
BY
	: WHITESPACE? (B Y) WHITESPACE?;
MODIFY
	: WHITESPACE? (M O D I F Y) WHITESPACE?;
SET
	: WHITESPACE? (S E T) WHITESPACE?;
TO
	: WHITESPACE? (T O) WHITESPACE?;
TRUE 
	: WHITESPACE? (T R U E) WHITESPACE?;
FALSE 
	: WHITESPACE? (F A L S E) WHITESPACE?; 
NEGATION
	: WHITESPACE? ((N O T)|'!') WHITESPACE?;
MINUS
	: WHITESPACE? '-' WHITESPACE?;
PLUS
	: WHITESPACE? '+' WHITESPACE?;
MULTIPLY
	: WHITESPACE? '*' WHITESPACE?;
DIVIDE
	: WHITESPACE? '/' WHITESPACE?;
EQUALTO
	: WHITESPACE? ((I S)|'=') WHITESPACE?;
LEFT_PAREN
	: WHITESPACE? '(' WHITESPACE?;
RIGHT_PAREN
	: WHITESPACE? ')' WHITESPACE?;

INTEGER
	: [1-9] [0-9]*;
NAME
	: [a-zA-Z_] [a-zA-Z0-9_]*;

DOT : '.';
QUOTE : '\'';
DBLQUOTE : '\"';

SPACE
	: ' ';
LOWERCASE 
	: [a-z];
UPPERCASE 
	: [A-Z];

NEWLINE	
	: WHITESPACE* ('\r'? '\n' | '\r')+ ( ('\r'? '\n' | '\r')+ | WHITESPACE)*;

fragment A : ('A'|'a');
fragment B : ('B'|'b');
fragment C : ('C'|'c');
fragment D : ('D'|'d');
fragment E : ('E'|'e');
fragment F : ('F'|'f');
fragment G : ('G'|'g');
fragment H : ('H'|'h');
fragment I : ('I'|'i');
fragment L : ('L'|'l');
fragment M : ('M'|'m');
fragment N : ('N'|'n');
fragment O : ('O'|'o');
fragment P : ('P'|'p');
fragment S : ('S'|'s');
fragment R : ('R'|'r');
fragment T : ('T'|'t');
fragment U : ('U'|'u');
fragment W : ('W'|'w');
fragment Y : ('Y'|'y');


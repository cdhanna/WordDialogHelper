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
prog: (rule|conditionSet|NEWLINE)+ EOF;

conditionSet
	: ruleTitle conditions
	;

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
	: CONDITIONS NEWLINE ((booleanExpr|substitute) NEWLINE)+
	;
substitute
	: USE WHITESPACE referance
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
	| outcomePass
	;

outcomeSetter
	: (SET referance TO expression)
	| (referance WHITESPACE? EQUALTO WHITESPACE? expression)
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
outcomePass
	: PASS
	;

dialogLine
	: COLON (text|templatedText) NEWLINE multilineText NEWLINE
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
	: WHITESPACE? ( EQUALTO | GREATERTHAN | LESSTHAN | IS ) WHITESPACE?
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
	: allowedReferenceWords | allowedReferenceWords (referanceSeparator referance)?
	;
allowedReferenceWords
	: NAME | CONDITIONS | AS | TO | WITH | MODIFY | IS | INTEGER
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
	| (QUOTE_LEFT (text|WHITESPACE?) QUOTE_RIGHT)
	;


numLiteral
	: INTEGER
	;

text
	: (WHITESPACE? (NAME|INTEGER|DOT|WHITESPACE|TO|SET|PASS|COMMA|TRUE|USE|FALSE|QUOTE|MODIFY|BY|IS|AS|NEGATION|CONDITIONS|MAYBE|DOTDOTDOT | APOSTROPHIE) WHITESPACE?)+
	;
multilineText
	: (freeText | templatedText)*
	;
freeText
	: (WHITESPACE | NAME | NEWLINE | TO | SET | BY | CONDITIONS | INTEGER |IS
	| MODIFY |COMMA | EQUALTO | NEGATION | MAYBE | FALSE | AS | WITH | USE
	| TRUE | DOT | POUND | LESSTHAN | GREATERTHAN |DOTDOTDOT | APOSTROPHIE
	| DIVIDE | QUOTE | DBLQUOTE | PASS)+
	;

templatedText
	: (LEFT_BRACKET expression RIGHT_BRACKET)
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
	| USE
	| PASS
	|IS
	;

/*
 * Lexer Rules
 */

COLON : ':';
COMMA: ',';
CONDITIONS	: C O N D I T I O N S   ; 
DISPLAYAS	: D I S P L A Y   A S   ;
DIALOGS		: D I A L O G S         ;
OUTCOMES	: O U T C O M E S       ;
USE			: U S E					;
PASS		: P A S S				;
WHITESPACE          
	: (' '|'\t')+ ;

GREATERTHAN
	: '>';
LESSTHAN
	: '<';


LEFT_BRACKET
	: '{' WHITESPACE?;
RIGHT_BRACKET
	: WHITESPACE? '}' ;

MAYBE
	: WHITESPACE? ('?') WHITESPACE?;
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
IS
	:  (I S) ;
EQUALTO
	:  ('=') ;
LEFT_PAREN
	: WHITESPACE? '(' WHITESPACE?;
RIGHT_PAREN
	: WHITESPACE? ')' WHITESPACE?;
POUND
	: WHITESPACE? '#' WHITESPACE?;


INTEGER
	: ([1-9] [0-9]*)|'0';
NAME
	: [a-zA-Z_] [a-zA-Z0-9_]*;

DOT : '.';
QUOTE_LEFT: '‘';
QUOTE_RIGHT: '’';
QUOTE : '\'';
DBLQUOTE : '\"';
APOSTROPHIE : '’';
DOTDOTDOT : '…';

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


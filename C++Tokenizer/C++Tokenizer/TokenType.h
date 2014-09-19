//
//  TokenType.h
//  tokenizer
//
//  Created by Alex Nagelkerke on 19-09-14.
//  Copyright (c) 2014 Alex Nagelkerke. All rights reserved.
//

#ifndef TOKENTYPE_H_INCLUDED
#define TOKENTYPE_H_INCLUDED

enum TokenType {
	OpenBracket,
	CloseBracket,
	OpenCurlyBracket,
	CloseCurlyBracket,
	StartIndex,
	EndOfIndex,
	Equals,
	EndOfStatement,
	Integer,
	Double,
	GreaterThan,
	LowerThan,
	OperatorPlus,
	OperatorMinus,
	OperatorDivide,
	OperatorRaised,
	OperatorMultiply,
	Keyword,
	Identifier,
	Function
};

#endif
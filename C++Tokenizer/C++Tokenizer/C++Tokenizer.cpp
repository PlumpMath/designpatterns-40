//
//  main.cpp
//  tokenizer
//
//  Created by Alex Nagelkerke on 19-09-14.
//  Copyright (c) 2014 Alex Nagelkerke. All rights reserved.
//

#include <iostream>
#include "Tokenizer.h"

int _tmain(int argc, const char* argv[])
{
	std::list<TokenDefinition> *definitions = new std::list<TokenDefinition>();
	definitions->push_back(*new TokenDefinition("if", TokenType::Keyword));
	definitions->push_back(*new TokenDefinition("else", TokenType::Keyword));
	definitions->push_back(*new TokenDefinition("[a-zA-Z][a-zA-Z0-9_]*", TokenType::Identifier));
	definitions->push_back(*new TokenDefinition("sin|cos|exp|ln|sqrt", TokenType::Function));
	definitions->push_back(*new TokenDefinition("\\(", TokenType::OpenBracket));
	definitions->push_back(*new TokenDefinition("\\)", TokenType::CloseBracket));
	definitions->push_back(*new TokenDefinition("[+-](?![+-])", TokenType::OperatorPlus));
	definitions->push_back(*new TokenDefinition("\\+{2}", TokenType::OperatorPlus));
	definitions->push_back(*new TokenDefinition("[-]{2}", TokenType::OperatorMinus));
	definitions->push_back(*new TokenDefinition("[*/]{1}", TokenType::OperatorMultiply));
	definitions->push_back(*new TokenDefinition("\\^", TokenType::OperatorRaised));
	definitions->push_back(*new TokenDefinition("[0-9]+", TokenType::Integer));
	definitions->push_back(*new TokenDefinition("\\{", TokenType::OpenCurlyBracket));
	definitions->push_back(*new TokenDefinition("\\}", TokenType::CloseCurlyBracket));
	definitions->push_back(*new TokenDefinition("\\[", TokenType::StartIndex));
	definitions->push_back(*new TokenDefinition("\\]", TokenType::EndOfIndex));
	definitions->push_back(*new TokenDefinition("\\<", TokenType::LowerThan));
	definitions->push_back(*new TokenDefinition("\\>", TokenType::GreaterThan));
	definitions->push_back(*new TokenDefinition("\\=", TokenType::Equals));
	definitions->push_back(*new TokenDefinition("\\;", TokenType::EndOfStatement));

	std::list<TokenPartner> *partners = new std::list<TokenPartner>();
	partners->push_back(*new TokenPartner("else", "if"));
	partners->push_back(*new TokenPartner(")", "("));
	partners->push_back(*new TokenPartner("}", "{"));

	Tokenizer *tokenizer = new Tokenizer("/Users/Alex/test.txt", *definitions, *partners);
	tokenizer->Tokenize();

	std::list<Token> *tokens = tokenizer->GetTokenList();
	std::list<Token>::iterator tokenIt;
	for (tokenIt = tokens->begin(); tokenIt != tokens->end(); ++tokenIt)
	{
		Token token = *tokenIt;
		Token partner;
		if (token.Partner)
			partner = *token.Partner;

		std::cout << "line: " + std::to_string(token.LineNumber) + " line position: " + std::to_string(token.LinePosition) + " level: " + std::to_string(token.Level) + " type: " + std::to_string(token.Type) + " Value: " + token.Value + " Partner: " + partner.Value << std::endl;
	}

	return 0;
}


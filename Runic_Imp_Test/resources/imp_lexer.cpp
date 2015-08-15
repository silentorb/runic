#include "Imp_Lexer.h"

Imp_Lexer::Imp_Lexer() {
  add_whisper(new runic::lexer::Regex_Whisper("spaces", "\G[ \t]+"));
  add_whisper(new runic::lexer::Whisper_Group("comma_or_newline", {new runic::lexer::Regex_Whisper("newlines", "\G(\s*\n)+\s*"), new runic::lexer::String_Whisper("comma", ",")}));
  add_whisper(new runic::lexer::Whisper_Group("semicolon_or_newline", {new runic::lexer::Regex_Whisper("newlines", "\G(\s*\n)+\s*"), new runic::lexer::String_Whisper("semicolon", ";")}));
  add_whisper(new runic::lexer::Regex_Whisper("line_comment", "\G\/\/[^\r\n]*"));
  add_whisper(new runic::lexer::Regex_Whisper("string_value", "\G"([^"]*)"|\G'([^']*)'"));
  add_whisper(new runic::lexer::Whisper_Group("bool_value", {new runic::lexer::String_Whisper("true", "true"), new runic::lexer::String_Whisper("false", "false")}));
  add_whisper(new runic::lexer::Regex_Whisper("float_value", "\G-?(?:[0-9]*\.)?[0-9]+"));
  add_whisper(new runic::lexer::Regex_Whisper("int_value", "\G-?[0-9]+"));
  add_whisper(new runic::lexer::String_Whisper("path_separator", "."));
  add_whisper(new runic::lexer::String_Whisper("arrow", "=>"));
  add_whisper(new runic::lexer::Regex_Whisper("insert", "\G\$\w*"));
  add_whisper(new runic::lexer::Whisper_Group("complex_set_operator", {new runic::lexer::String_Whisper("+=", "+="), new runic::lexer::String_Whisper("-=", "-="), new runic::lexer::String_Whisper("*=", "*="), new runic::lexer::String_Whisper("/=", "/="), new runic::lexer::String_Whisper("@=", "@=")}));
  add_whisper(new runic::lexer::Whisper_Group("operator_token", {new runic::lexer::String_Whisper("+", "+"), new runic::lexer::String_Whisper("-", "-"), new runic::lexer::String_Whisper("/", "/"), new runic::lexer::String_Whisper("*", "*"), new runic::lexer::String_Whisper("<=", "<="), new runic::lexer::String_Whisper(">=", ">="), new runic::lexer::String_Whisper("<", "<"), new runic::lexer::String_Whisper(">", ">"), new runic::lexer::String_Whisper("==", "=="), new runic::lexer::String_Whisper("!=", "!="), new runic::lexer::String_Whisper("&&", "&&"), new runic::lexer::String_Whisper("||", "||")}));
  add_whisper(new runic::lexer::Whisper_Group("set_operator", {new runic::lexer::String_Whisper("equals", "="), whispers["complex_set_operator"]}));
  add_whisper(new runic::lexer::String_Whisper("meta_list_start", "@["));
  add_whisper(new runic::lexer::String_Whisper("block_start", "{"));
  add_whisper(new runic::lexer::String_Whisper("block_end", "}"));
  add_whisper(new runic::lexer::String_Whisper("group_start", "("));
  add_whisper(new runic::lexer::String_Whisper("group_end", ")"));
  add_whisper(new runic::lexer::String_Whisper("array_start", "["));
  add_whisper(new runic::lexer::String_Whisper("array_end", "]"));
  add_whisper(new runic::lexer::String_Whisper("colon", ":"));
  add_whisper(new runic::lexer::Regex_Whisper("preprocessor_start", "\G#[a-zA-Z0-9_]+"));
  add_whisper(new runic::lexer::Regex_Whisper("id_token", "\G[a-zA-Z0-9_]+"));
  add_whisper(new runic::lexer::Whisper_Group("keyword", {new runic::lexer::String_Whisper("abstract", "abstract"), new runic::lexer::String_Whisper("break", "break"), new runic::lexer::String_Whisper("catch", "catch"), new runic::lexer::String_Whisper("class", "class"), new runic::lexer::String_Whisper("const", "const"), new runic::lexer::String_Whisper("continue", "continue"), new runic::lexer::String_Whisper("delete", "delete"), new runic::lexer::String_Whisper("do", "do"), new runic::lexer::String_Whisper("else", "else"), new runic::lexer::String_Whisper("enum", "enum"), new runic::lexer::String_Whisper("export", "export"), new runic::lexer::String_Whisper("external", "external"), new runic::lexer::String_Whisper("if", "if"), new runic::lexer::String_Whisper("import", "import"), new runic::lexer::String_Whisper("include", "include"), new runic::lexer::String_Whisper("in", "in"), new runic::lexer::String_Whisper("finally", "finally"), new runic::lexer::String_Whisper("for", "for"), new runic::lexer::String_Whisper("namespace", "namespace"), new runic::lexer::String_Whisper("new", "new"), new runic::lexer::String_Whisper("null", "null"), new runic::lexer::String_Whisper("private", "private"), new runic::lexer::String_Whisper("public", "public"), new runic::lexer::String_Whisper("return", "return"), new runic::lexer::String_Whisper("static", "static"), new runic::lexer::String_Whisper("struct", "struct"), new runic::lexer::String_Whisper("throw", "throw"), new runic::lexer::String_Whisper("while", "while"), new runic::lexer::String_Whisper("var", "var")}));
  add_whisper(new runic::lexer::Whisper_Group("type", {new runic::lexer::String_Whisper("bool", "bool"), new runic::lexer::String_Whisper("int", "int"), new runic::lexer::String_Whisper("float", "float"), new runic::lexer::String_Whisper("string", "string")}));
  add_whisper(new runic::lexer::Regex_Whisper("ws", "\G[ \t]+"));
}

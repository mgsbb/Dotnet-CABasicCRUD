export function pascalToCamel(input: string): string {
  if (!input) return "";
  return input[0].toLowerCase() + input.slice(1);
}

import Cookies from "js-cookie";

export function salvarToken(token: string) {
  Cookies.set("token", token, { expires: 1 });
}

export function obterToken() {
  return Cookies.get("token");
}

export function removerToken() {
  Cookies.remove("token");
}

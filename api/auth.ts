import {PATH_PREFIX} from "@/api/index";
import {UserModel} from "@/models/user";
import {cookies} from "next/headers";

export async function getUserProfileAPi() {
  const response = await fetch(`${PATH_PREFIX}/api/auth/profile`, {
    headers: {
      ["Cookie"]: cookies().toString()
    }
  });
  if (!response.ok)
    return null;
  return await response.json() as UserModel;
}
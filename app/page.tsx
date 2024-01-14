import Link from "next/link";
import {Button} from "@/components/ui/button";
import {getUserProfileAPi} from "@/api/auth";

export default async function Home() {
  const userModel = await getUserProfileAPi();

  return (
    <>
      <p>
        {
          !userModel &&
            <Link href={"/api/auth/login"}>
                <Button variant={"link"}>Sign In</Button>
            </Link>
        }
        {
          userModel &&
            <Link href={"/api/auth/logout"}>
                <Button variant={"link"}>Sign Out</Button>
            </Link>
        }
      </p>
      {userModel && (
        <>
          <p>External Id: {userModel.externalId}</p>
          <p>Name: {userModel.name}</p>
          <p>Email: {userModel.emailAddress}</p>
        </>
      )}
    </>
  )
}

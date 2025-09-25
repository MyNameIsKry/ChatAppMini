import Link from "next/link";
import { Button } from "@/components/ui/button";
import { MessageCircle } from "lucide-react";
import UserReview from "@/components/home/userReview";
import { Navigation } from "@/components/home/navigation";
import Feature from "@/components/home/feature";
import axios from "axios";
import { cookies } from "next/headers";

export default async function Home() {
  const userData = await axios.get("http://localhost:5189/api/users/@me", {
    headers: {
      Authorization: `Bearer ${(await cookies()).get("accessToken")?.value}`,
    },
    withCredentials: true
  });

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 via-indigo-50 to-purple-50">
      <header className="sticky top-0 z-50 backdrop-blur-sm">
        <div className="container mx-auto px-4 py-4">
          <Navigation userData={userData.data.data} />
        </div>
      </header>

      <main className="pt-16">
        <div className="text-center">
          <h1 className="text-5xl font-bold text-gray-900 mb-6">
            Kết nối và trò chuyện
            <br />
            <span className="text-blue-600">mọi lúc, mọi nơi</span>
          </h1>
          <p className="text-xl text-gray-600 mb-8 max-w-2xl mx-auto">
            ChatMini là ứng dụng chat hiện đại, giúp bạn kết nối với bạn bè, 
            gia đình và đồng nghiệp một cách dễ dàng và an toàn.
          </p>
        </div>
        <UserReview />
      </main>

      <main className="container mx-auto px-4 pb-16 py-12">
        <Feature />
        <div className="mt-24 text-center bg-white rounded-lg p-12 shadow-lg">
          <h2 className="text-3xl font-bold text-gray-900 mb-4">
            Sẵn sàng bắt đầu cuộc trò chuyện?
          </h2>
          <p className="text-lg text-gray-600 mb-8">
            Tham gia cùng hàng ngàn người dùng đang sử dụng ChatMini
          </p>
          <Link href="/register">
            <Button size="lg" className="px-12 py-4 text-lg cursor-pointer">
              Tạo tài khoản miễn phí
            </Button>
          </Link>
        </div>
      </main>

      <footer className="bg-gray-900 text-white py-8 mt-16">
        <div className="container mx-auto px-4 text-center">
          <div className="flex items-center justify-center space-x-2 mb-4">
            <MessageCircle className="h-6 w-6" />
            <span className="text-lg font-semibold">ChatMini</span>
          </div>
          <p className="text-gray-400">
            © 2025 ChatMini. Tất cả quyền được bảo lưu.
          </p>
        </div>
      </footer>
    </div>
  );
}

"use client";
import Link from "next/link";
import { Button } from "@/components/ui/button";
import { MessageCircle, Users, Shield, Zap } from "lucide-react";
import { cn } from "@/lib/utils";
import { Marquee } from "@/components/magicui/marquee";
import reviews from "../../data/reviews.json";
import { motion } from "motion/react";  

export default function Home() {
  const firstRow = reviews.slice(0, reviews.length / 2);
  const secondRow = reviews.slice(reviews.length / 2);

  const ReviewCard = ({
    img,
    name,
    username,
    body,
  }: {
    img: string;
    name: string;
    username: string;
    body: string;
  }) => {
    return (
      <figure
        className={cn(
          "relative h-full w-64 cursor-pointer overflow-hidden rounded-xl border p-4",
          "border-gray-950/[.1] bg-gray-950/[.01] hover:bg-gray-950/[.05]",
          "dark:border-gray-50/[.1] dark:bg-gray-50/[.10] dark:hover:bg-gray-50/[.15]",
        )}
      >
        <div className="flex flex-row items-center gap-2">
          <img className="rounded-full" width="32" height="32" alt="" src={img} />
          <div className="flex flex-col">
            <figcaption className="text-sm font-medium dark:text-white">
              {name}
            </figcaption>
            <p className="text-xs font-medium dark:text-white/40">{username}</p>
          </div>
        </div>
        <blockquote className="mt-2 text-sm">{body}</blockquote>
      </figure>
    );
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
      <header className="container mx-auto px-4 py-6">
        <nav className="flex items-center justify-between">
          <div className="flex items-center space-x-2">
            <MessageCircle className="h-8 w-8 text-blue-600" />
            <span className="text-2xl font-bold text-gray-900">ChatMini</span>
          </div>
          <div className="space-x-4">
            <Link href="/login">
              <Button variant="outline" className="cursor-pointer">Đăng nhập</Button>
            </Link>
            <Link href="/register">
              <Button className="cursor-pointer">Đăng ký</Button>
            </Link>
          </div>
        </nav>
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
          <div className="relative flex w-full flex-col items-center justify-center overflow-hidden">
            <Marquee pauseOnHover className="[--duration:20s]">
              {firstRow.map((review) => (
                <ReviewCard key={review.username} {...review} />
              ))}
            </Marquee>
            <Marquee reverse pauseOnHover className="[--duration:20s]">
              {secondRow.map((review) => (
                <ReviewCard key={review.username} {...review} />
              ))}
            </Marquee>
            <div className="pointer-events-none absolute inset-y-0 left-0 w-1/4"></div>
            <div className="pointer-events-none absolute inset-y-0 right-0 w-1/4"></div>
          </div>
        </div>
      </main>

      <main className="container mx-auto px-4 pb-16 py-12">
        <div className="mt-24 grid md:grid-cols-3 gap-8">
          {[
            {
              icon: Users,
              title: "Chat nhóm",
              description: "Tạo nhóm chat với bạn bè, chia sẻ khoảnh khắc và thông tin quan trọng."
            },
            {
              icon: Shield,
              title: "Bảo mật cao",
              description: "Tin nhắn được mã hóa end-to-end, đảm bảo riêng tư và an toàn tuyệt đối."
            },
            {
              icon: Zap,
              title: "Thời gian thực",
              description: "Nhận và gửi tin nhắn ngay lập tức với công nghệ SignalR hiện đại."
            }
          ].map((feature, index) => {
            const IconComponent = feature.icon;
            return (
              <motion.div
                key={index}
                initial={{ opacity: 0, y: 50 }}
                whileInView={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.5, delay: index * 0.25 }}
                viewport={{ once: true }}
              >
                <div className="text-center p-6 bg-white rounded-lg shadow-md">
                  <IconComponent className="h-12 w-12 text-blue-600 mx-auto mb-4" />
                  <h3 className="text-xl font-semibold mb-2">{feature.title}</h3>
                  <p className="text-gray-600">{feature.description}</p>
                </div>
              </motion.div>
            );
          })}
        </div>

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

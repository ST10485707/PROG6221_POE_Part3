// ============================================================
// ChatbotLogic.cs - NLP Simulation & Keyword Recognition
// Student: ST10485707
// Description:
//   This class handles keyword detection for cybersecurity topics.
//   It recognises natural language variations and provides
//   relevant cybersecurity advice.
//   Keywords covered:
//   - Password, 2FA, authentication, login security
//   - Scam, fraud, phishing, suspicious links, email scam
//   - Privacy, data breach, personal info, social engineering
//   - Malware, virus, ransomware, safe browsing, public Wi-Fi
// ============================================================

using System;
using System.Collections.Generic;

namespace PROG6221_POE_Part3
{
    public class ChatbotLogic
    {
        private Random random = new Random();

        // ========== RANDOM RESPONSE LISTS ==========

        private string[] passwordResponses = {
            "🔐 Use at least 12 characters with uppercase, lowercase, numbers, and symbols.",
            "⚠️ Never reuse passwords across different accounts. Use a password manager!",
            "📅 Change important passwords every 3-6 months.",
            "🚫 Avoid using personal info like your name, birthday, or 'password123'.",
            "🔑 Enable Two-Factor Authentication (2FA) for an extra layer of security!",
            "🛡️ Use a password manager to generate and store strong unique passwords."
        };

        private string[] scamResponses = {
            "🚨 Scam alert! Never share your OTP or PIN with anyone. Banks will never ask for this.",
            "📞 Watch for fake calls! Scammers pretend to be from 'your bank'. Hang up and call the official number.",
            "📧 Email scams often have spelling mistakes. Check the sender's email address carefully!",
            "💰 'You won a prize!' is almost always a scam. Never pay money to receive 'winnings'.",
            "🔗 Hover over links before clicking to see the real destination.",
            "🛑 If something feels too good to be true, it probably is. Trust your instincts!"
        };

        private string[] privacyResponses = {
            "🔒 Review your privacy settings on social media at least once a month.",
            "🌐 Use private browsing (Incognito) when searching sensitive topics.",
            "📱 Check which apps have access to your location, camera, and microphone.",
            "🛡️ Two-factor authentication (2FA) adds an extra layer of privacy protection.",
            "🔑 Use strong, unique passwords for each account to protect your personal data.",
            "📧 Be careful what personal information you share in emails or online forms."
        };

        private string[] phishingResponses = {
            "🎣 Phishing emails often create urgency: 'Your account will be closed!'",
            "✉️ Hover over links before clicking to see the real destination.",
            "🛑 Never download attachments from unknown senders.",
            "✅ Legitimate companies address you by name, not 'Dear Customer'.",
            "🔗 Check the sender's email address carefully - scammers use fake addresses.",
            "📞 If you receive a suspicious call, hang up and call the official number directly."
        };

        private string[] browsingResponses = {
            "🌐 Look for 'https://' and a padlock icon in the address bar.",
            "📡 Avoid public Wi-Fi for banking or shopping. Use a VPN if needed.",
            "🔄 Keep your browser and extensions updated for security patches.",
            "🚫 Don't save passwords in your browser. Use a dedicated password manager.",
            "🛡️ Use a reputable antivirus and firewall for real-time protection.",
            "🔒 Enable private browsing mode when accessing sensitive information."
        };

        private string[] socialEngineeringResponses = {
            "🧠 Social engineering manipulates people into revealing confidential information.",
            "📞 Scammers impersonate trusted organisations to gain your trust.",
            "🔗 Never click on links from unknown sources - they could lead to fake login pages.",
            "🛑 Always verify the identity of the person contacting you before sharing any info.",
            "📧 Be wary of urgent requests for money or personal information."
        };

        private string[] malwareResponses = {
            "💻 Install a reputable antivirus and keep it updated.",
            "📎 Never open email attachments from unknown senders.",
            "🔄 Keep your operating system and software updated for security patches.",
            "📱 Be careful what you download - only use official app stores.",
            "🛡️ Use a firewall to block unauthorised access to your device."
        };

        private string[] twoFAResponses = {
            "📱 Two-Factor Authentication (2FA) adds a second verification step.",
            "🔐 Even if someone gets your password, they can't access your account without the 2FA code.",
            "📲 Use an authenticator app like Google Authenticator or Microsoft Authenticator.",
            "🚫 Never share your 2FA codes with anyone - not even 'support' staff.",
            "✅ Enable 2FA on all important accounts: email, banking, and social media."
        };

        private string[] dataBreachResponses = {
            "🔍 Check if your email has been in a data breach using sites like HaveIBeenPwned.",
            "⚠️ If your data has been breached, change your passwords immediately.",
            "🛡️ Enable Two-Factor Authentication (2FA) on all important accounts.",
            "📧 Be extra cautious of phishing emails after a data breach.",
            "🔐 Monitor your bank accounts for any suspicious activity."
        };

        private string[] defaultResponses = {
            "I can help with passwords, scams, privacy, phishing, safe browsing, social engineering, malware, 2FA, and data breaches. What would you like to know?",
            "Ask me about online safety! Topics: passwords, scams, privacy, phishing, social engineering, malware, 2FA, or data breaches.",
            "I'm here to help you stay safe online. Try asking about 'password', 'scam', 'privacy', 'phishing', or '2FA'.",
            "Cybersecurity is important! I can help with passwords, scams, privacy, phishing, social engineering, malware, 2FA, or data breaches."
        };

        // ========== KEYWORD DETECTION ==========

        public string GetBotResponse(string input)
        {
            input = input.ToLower();

            // ========== KEYWORD VARIATIONS ==========

            // --- PASSWORD & 2FA ---
            if (input.Contains("password") || input.Contains("passwords") ||
                input.Contains("login") || input.Contains("authentication") ||
                input.Contains("account security") || input.Contains("secure password"))
            {
                return GetRandomResponse(passwordResponses);
            }

            // --- TWO-FACTOR AUTHENTICATION ---
            if (input.Contains("2fa") || input.Contains("two factor") ||
                input.Contains("two-factor") || input.Contains("2 factor") ||
                input.Contains("multi factor") || input.Contains("multi-factor") ||
                input.Contains("mfa") || input.Contains("authenticator"))
            {
                return GetRandomResponse(twoFAResponses);
            }

            // --- SCAM & FRAUD ---
            if (input.Contains("scam") || input.Contains("scams") ||
                input.Contains("fraud") || input.Contains("scammer") ||
                input.Contains("fake") || input.Contains("con artist"))
            {
                return GetRandomResponse(scamResponses);
            }

            // --- SOCIAL ENGINEERING ---
            if (input.Contains("social engineering") || input.Contains("manipulation") ||
                input.Contains("impersonate") || input.Contains("impersonation") ||
                input.Contains("trust") || input.Contains("verify identity"))
            {
                return GetRandomResponse(socialEngineeringResponses);
            }

            // --- PHISHING & SUSPICIOUS LINKS ---
            if (input.Contains("phish") || input.Contains("phishing") ||
                input.Contains("suspicious link") || input.Contains("suspicious links") ||
                input.Contains("email scam") || input.Contains("email security") ||
                input.Contains("click link") || input.Contains("suspicious email"))
            {
                return GetRandomResponse(phishingResponses);
            }

            // --- DATA BREACH ---
            if (input.Contains("data breach") || input.Contains("breach") ||
                input.Contains("hacked") || input.Contains("compromised") ||
                input.Contains("security breach") || input.Contains("data leak"))
            {
                return GetRandomResponse(dataBreachResponses);
            }

            // --- MALWARE & VIRUS ---
            if (input.Contains("malware") || input.Contains("virus") ||
                input.Contains("ransomware") || input.Contains("trojan") ||
                input.Contains("spyware") || input.Contains("antivirus") ||
                input.Contains("malicious") || input.Contains("infected"))
            {
                return GetRandomResponse(malwareResponses);
            }

            // --- PRIVACY ---
            if (input.Contains("privacy") || input.Contains("private") ||
                input.Contains("personal info") || input.Contains("personal information") ||
                input.Contains("data protection") || input.Contains("information security"))
            {
                return GetRandomResponse(privacyResponses);
            }

            // --- SAFE BROWSING ---
            if (input.Contains("browse") || input.Contains("browsing") ||
                input.Contains("safe browsing") || input.Contains("secure browsing") ||
                input.Contains("public wi-fi") || input.Contains("public wifi") ||
                input.Contains("https") || input.Contains("padlock") ||
                input.Contains("incognito") || input.Contains("private browsing"))
            {
                return GetRandomResponse(browsingResponses);
            }

            // --- DEFAULT ---
            return GetRandomResponse(defaultResponses);
        }

        private string GetRandomResponse(string[] responses)
        {
            int index = random.Next(responses.Length);
            return responses[index];
        }
    }
}
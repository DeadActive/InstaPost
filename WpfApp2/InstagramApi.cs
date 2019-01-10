using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WpfApp2
{
    public struct device_setting
    {
        string manufacturer;
        string model;
        int android_version;
        string android_release;

        public device_setting(string man, string mod, int ver, string rel)
        {
            manufacturer = man;
            model = mod;
            android_version = ver;
            android_release = rel;
        }
    }

    class InstagramApi
    {
        private const string API_URL = "https://i.instagram.com/api/v1/";
        private const string manufacturer = "Xiaomi";
        private const string model = "HM 1SW";
        private const int android_version = 18;
        private const string android_release = "4.3";
        private string USER_AGENT = string.Format("Instagram 10.26.0 Android ({0}/{1}; 320dpi; 720x1280; {2}; {3}; armani; qcom; en_US)",
            android_version, android_release, manufacturer, model);
        private const string IG_SIG_KEY = "4f8732eb9ba7d1c8e8897a75d6474d4eb3f5279137431b2aafb71fafe2abe178";
        private const string EXPERIMENTS = "ig_promote_reach_objective_fix_universe,ig_android_universe_video_production,ig_search_client_h1_2017_holdout,ig_android_live_follow_from_comments_universe,ig_android_carousel_non_square_creation,ig_android_live_analytics,ig_android_follow_all_dialog_confirmation_copy,ig_android_stories_server_coverframe,ig_android_video_captions_universe,ig_android_offline_location_feed,ig_android_direct_inbox_retry_seen_state,ig_android_ontact_invite_universe,ig_android_live_broadcast_blacklist,ig_android_insta_video_reconnect_viewers,ig_android_ad_async_ads_universe,ig_android_search_clear_layout_universe,ig_android_shopping_reporting,ig_android_stories_surface_universe,ig_android_verified_comments_universe,ig_android_preload_media_ahead_in_current_reel,android_instagram_prefetch_suggestions_universe,ig_android_reel_viewer_fetch_missing_reels_universe,ig_android_direct_search_share_sheet_universe,ig_android_business_promote_tooltip,ig_android_direct_blue_tab,ig_android_async_network_tweak_universe,ig_android_elevate_main_thread_priority_universe,ig_android_stories_gallery_nux,ig_android_instavideo_remove_nux_comments,ig_video_copyright_whitelist,ig_react_native_inline_insights_with_relay,ig_android_direct_thread_message_animation,ig_android_draw_rainbow_client_universe,ig_android_direct_link_style,ig_android_live_heart_enhancements_universe,ig_android_rtc_reshare,ig_android_preload_item_count_in_reel_viewer_buffer,ig_android_users_bootstrap_service,ig_android_auto_retry_post_mode,ig_android_shopping,ig_android_main_feed_seen_state_dont_send_info_on_tail_load,ig_fbns_preload_default,ig_android_gesture_dismiss_reel_viewer,ig_android_tool_tip,ig_android_ad_logger_funnel_logging_universe,ig_android_gallery_grid_column_count_universe,ig_android_business_new_ads_payment_universe,ig_android_direct_links,ig_android_audience_control,ig_android_live_encore_consumption_settings_universe,ig_perf_android_holdout,ig_android_cache_contact_import_list,ig_android_links_receivers,ig_android_ad_impression_backtest,ig_android_list_redesign,ig_android_stories_separate_overlay_creation,ig_android_stop_video_recording_fix_universe,ig_android_render_video_segmentation,ig_android_live_encore_reel_chaining_universe,ig_android_sync_on_background_enhanced_10_25,ig_android_immersive_viewer,ig_android_mqtt_skywalker,ig_fbns_push,ig_android_ad_watchmore_overlay_universe,ig_android_react_native_universe,ig_android_profile_tabs_redesign_universe,ig_android_live_consumption_abr,ig_android_story_viewer_social_context,ig_android_hide_post_in_feed,ig_android_video_loopcount_int,ig_android_enable_main_feed_reel_tray_preloading,ig_android_camera_upsell_dialog,ig_android_ad_watchbrowse_universe,ig_android_internal_research_settings,ig_android_search_people_tag_universe,ig_android_react_native_ota,ig_android_enable_concurrent_request,ig_android_react_native_stories_grid_view,ig_android_business_stories_inline_insights,ig_android_log_mediacodec_info,ig_android_direct_expiring_media_loading_errors,ig_video_use_sve_universe,ig_android_cold_start_feed_request,ig_android_enable_zero_rating,ig_android_reverse_audio,ig_android_branded_content_three_line_ui_universe,ig_android_live_encore_production_universe,ig_stories_music_sticker,ig_android_stories_teach_gallery_location,ig_android_http_stack_experiment_2017,ig_android_stories_device_tilt,ig_android_pending_request_search_bar,ig_android_fb_topsearch_sgp_fork_request,ig_android_seen_state_with_view_info,ig_android_animation_perf_reporter_timeout,ig_android_new_block_flow,ig_android_story_tray_title_play_all_v2,ig_android_direct_address_links,ig_android_stories_archive_universe,ig_android_save_collections_cover_photo,ig_android_live_webrtc_livewith_production,ig_android_sign_video_url,ig_android_stories_video_prefetch_kb,ig_android_stories_create_flow_favorites_tooltip,ig_android_live_stop_broadcast_on_404,ig_android_live_viewer_invite_universe,ig_android_promotion_feedback_channel,ig_android_render_iframe_interval,ig_android_accessibility_logging_universe,ig_android_camera_shortcut_universe,ig_android_use_one_cookie_store_per_user_override,ig_profile_holdout_2017_universe,ig_android_stories_server_brushes,ig_android_ad_media_url_logging_universe,ig_android_shopping_tag_nux_text_universe,ig_android_comments_single_reply_universe,ig_android_stories_video_loading_spinner_improvements,ig_android_collections_cache,ig_android_comment_api_spam_universe,ig_android_facebook_twitter_profile_photos,ig_android_shopping_tag_creation_universe,ig_story_camera_reverse_video_experiment,ig_android_direct_bump_selected_recipients,ig_android_ad_cta_haptic_feedback_universe,ig_android_vertical_share_sheet_experiment,ig_android_family_bridge_share,ig_android_search,ig_android_insta_video_consumption_titles,ig_android_stories_gallery_preview_button,ig_android_fb_auth_education,ig_android_camera_universe,ig_android_me_only_universe,ig_android_instavideo_audio_only_mode,ig_android_user_profile_chaining_icon,ig_android_live_video_reactions_consumption_universe,ig_android_stories_hashtag_text,ig_android_post_live_badge_universe,ig_android_swipe_fragment_container,ig_android_search_users_universe,ig_android_live_save_to_camera_roll_universe,ig_creation_growth_holdout,ig_android_sticker_region_tracking,ig_android_unified_inbox,ig_android_live_new_watch_time,ig_android_offline_main_feed_10_11,ig_import_biz_contact_to_page,ig_android_live_encore_consumption_universe,ig_android_experimental_filters,ig_android_search_client_matching_2,ig_android_react_native_inline_insights_v2,ig_android_business_conversion_value_prop_v2,ig_android_redirect_to_low_latency_universe,ig_android_ad_show_new_awr_universe,ig_family_bridges_holdout_universe,ig_android_background_explore_fetch,ig_android_following_follower_social_context,ig_android_video_keep_screen_on,ig_android_ad_leadgen_relay_modern,ig_android_profile_photo_as_media,ig_android_insta_video_consumption_infra,ig_android_ad_watchlead_universe,ig_android_direct_prefetch_direct_story_json,ig_android_shopping_react_native,ig_android_top_live_profile_pics_universe,ig_android_direct_phone_number_links,ig_android_stories_weblink_creation,ig_android_direct_search_new_thread_universe,ig_android_histogram_reporter,ig_android_direct_on_profile_universe,ig_android_network_cancellation,ig_android_background_reel_fetch,ig_android_react_native_insights,ig_android_insta_video_audio_encoder,ig_android_family_bridge_bookmarks,ig_android_data_usage_network_layer,ig_android_universal_instagram_deep_links,ig_android_dash_for_vod_universe,ig_android_modular_tab_discover_people_redesign,ig_android_mas_sticker_upsell_dialog_universe,ig_android_ad_add_per_event_counter_to_logging_event,ig_android_sticky_header_top_chrome_optimization,ig_android_rtl,ig_android_biz_conversion_page_pre_select,ig_android_promote_from_profile_button,ig_android_live_broadcaster_invite_universe,ig_android_share_spinner,ig_android_text_action,ig_android_own_reel_title_universe,ig_promotions_unit_in_insights_landing_page,ig_android_business_settings_header_univ,ig_android_save_longpress_tooltip,ig_android_constrain_image_size_universe,ig_android_business_new_graphql_endpoint_universe,ig_ranking_following,ig_android_stories_profile_camera_entry_point,ig_android_universe_reel_video_production,ig_android_power_metrics,ig_android_sfplt,ig_android_offline_hashtag_feed,ig_android_live_skin_smooth,ig_android_direct_inbox_search,ig_android_stories_posting_offline_ui,ig_android_sidecar_video_upload_universe,ig_android_promotion_manager_entry_point_universe,ig_android_direct_reply_audience_upgrade,ig_android_swipe_navigation_x_angle_universe,ig_android_offline_mode_holdout,ig_android_live_send_user_location,ig_android_direct_fetch_before_push_notif,ig_android_non_square_first,ig_android_insta_video_drawing,ig_android_swipeablefilters_universe,ig_android_live_notification_control_universe,ig_android_analytics_logger_running_background_universe,ig_android_save_all,ig_android_reel_viewer_data_buffer_size,ig_direct_quality_holdout_universe,ig_android_family_bridge_discover,ig_android_react_native_restart_after_error_universe,ig_android_startup_manager,ig_story_tray_peek_content_universe,ig_android_profile,ig_android_high_res_upload_2,ig_android_http_service_same_thread,ig_android_scroll_to_dismiss_keyboard,ig_android_remove_followers_universe,ig_android_skip_video_render,ig_android_story_timestamps,ig_android_live_viewer_comment_prompt_universe,ig_profile_holdout_universe,ig_android_react_native_insights_grid_view,ig_stories_selfie_sticker,ig_android_stories_reply_composer_redesign,ig_android_streamline_page_creation,ig_explore_netego,ig_android_ig4b_connect_fb_button_universe,ig_android_feed_util_rect_optimization,ig_android_rendering_controls,ig_android_os_version_blocking,ig_android_encoder_width_safe_multiple_16,ig_search_new_bootstrap_holdout_universe,ig_android_snippets_profile_nux,ig_android_e2e_optimization_universe,ig_android_comments_logging_universe,ig_shopping_insights,ig_android_save_collections,ig_android_live_see_fewer_videos_like_this_universe,ig_android_show_new_contact_import_dialog,ig_android_live_view_profile_from_comments_universe,ig_fbns_blocked,ig_formats_and_feedbacks_holdout_universe,ig_android_reduce_view_pager_buffer,ig_android_instavideo_periodic_notif,ig_search_user_auto_complete_cache_sync_ttl,ig_android_marauder_update_frequency,ig_android_suggest_password_reset_on_oneclick_login,ig_android_promotion_entry_from_ads_manager_universe,ig_android_live_special_codec_size_list,ig_android_enable_share_to_messenger,ig_android_background_main_feed_fetch,ig_android_live_video_reactions_creation_universe,ig_android_channels_home,ig_android_sidecar_gallery_universe,ig_android_upload_reliability_universe,ig_migrate_mediav2_universe,ig_android_insta_video_broadcaster_infra_perf,ig_android_business_conversion_social_context,android_ig_fbns_kill_switch,ig_android_live_webrtc_livewith_consumption,ig_android_destroy_swipe_fragment,ig_android_react_native_universe_kill_switch,ig_android_stories_book_universe,ig_android_all_videoplayback_persisting_sound,ig_android_draw_eraser_universe,ig_direct_search_new_bootstrap_holdout_universe,ig_android_cache_layer_bytes_threshold,ig_android_search_hash_tag_and_username_universe,ig_android_business_promotion,ig_android_direct_search_recipients_controller_universe,ig_android_ad_show_full_name_universe,ig_android_anrwatchdog,ig_android_qp_kill_switch,ig_android_2fac,ig_direct_bypass_group_size_limit_universe,ig_android_promote_simplified_flow,ig_android_share_to_whatsapp,ig_android_hide_bottom_nav_bar_on_discover_people,ig_fbns_dump_ids,ig_android_hands_free_before_reverse,ig_android_skywalker_live_event_start_end,ig_android_live_join_comment_ui_change,ig_android_direct_search_story_recipients_universe,ig_android_direct_full_size_gallery_upload,ig_android_ad_browser_gesture_control,ig_channel_server_experiments,ig_android_video_cover_frame_from_original_as_fallback,ig_android_ad_watchinstall_universe,ig_android_ad_viewability_logging_universe,ig_android_new_optic,ig_android_direct_visual_replies,ig_android_stories_search_reel_mentions_universe,ig_android_threaded_comments_universe,ig_android_mark_reel_seen_on_Swipe_forward,ig_internal_ui_for_lazy_loaded_modules_experiment,ig_fbns_shared,ig_android_capture_slowmo_mode,ig_android_live_viewers_list_search_bar,ig_android_video_single_surface,ig_android_offline_reel_feed,ig_android_video_download_logging,ig_android_last_edits,ig_android_exoplayer_4142,ig_android_post_live_viewer_count_privacy_universe,ig_android_activity_feed_click_state,ig_android_snippets_haptic_feedback,ig_android_gl_drawing_marks_after_undo_backing,ig_android_mark_seen_state_on_viewed_impression,ig_android_live_backgrounded_reminder_universe,ig_android_live_hide_viewer_nux_universe,ig_android_live_monotonic_pts,ig_android_search_top_search_surface_universe,ig_android_user_detail_endpoint,ig_android_location_media_count_exp_ig,ig_android_comment_tweaks_universe,ig_android_ad_watchmore_entry_point_universe,ig_android_top_live_notification_universe,ig_android_add_to_last_post,ig_save_insights,ig_android_live_enhanced_end_screen_universe,ig_android_ad_add_counter_to_logging_event,ig_android_blue_token_conversion_universe,ig_android_exoplayer_settings,ig_android_progressive_jpeg,ig_android_offline_story_stickers,ig_android_gqls_typing_indicator,ig_android_chaining_button_tooltip,ig_android_video_prefetch_for_connectivity_type,ig_android_use_exo_cache_for_progressive,ig_android_samsung_app_badging,ig_android_ad_holdout_watchandmore_universe,ig_android_offline_commenting,ig_direct_stories_recipient_picker_button,ig_insights_feedback_channel_universe,ig_android_insta_video_abr_resize,ig_android_insta_video_sound_always_on";
        private const string SIG_KEY_VERSION = "4";

        private string username;
        private string password;
        private string device_id;
        private string uuid;
        private HttpWebRequest req;
        private readonly HttpClient http;
        private HttpClientHandler handler;
        private CookieContainer cookies;
        private CookieCollection resposeCookies;
        private HttpResponseMessage lastResponse;
        private JObject lastJson;

        private bool isLoggedIn;
        private string username_id;
        private string rank_token;
        private string token;

        //public section
        public bool loggedIn = false;
        public bool fail = false;

        public InstagramApi(string username, string password, bool Debug = false)
        {
            var m = MD5.Create();
            var up = new byte[Encoding.UTF8.GetBytes(username).Length + Encoding.UTF8.GetBytes(password).Length];
            Encoding.UTF8.GetBytes(username).CopyTo(up, 0);
            Encoding.UTF8.GetBytes(password).CopyTo(up, Encoding.UTF8.GetBytes(username).Length);
            m.ComputeHash(up);
            device_id = generateDeviceId(BitConverter.ToString(m.Hash).Replace("-", ""));
            setUser(username, password);
            this.isLoggedIn = false;
            this.cookies = new CookieContainer();
            this.handler = new HttpClientHandler
            {
                CookieContainer = cookies,
                UseCookies = true,
                UseDefaultCredentials = false
            };
            this.http = new HttpClient(this.handler);
        }

        public void setUser(string username, string password)
        {
            this.username = username;
            this.password = password;
            this.uuid = generateUUID(true);
        }

        public async void relogin(int attempt, bool force = false)
        {
            if (!this.isLoggedIn || force)
            {
                bool req = await SendRequest("si/fetch_headers/?challenge_type=signup&guid=" + generateUUID(false), null, true);
                if (req)
                {
                    Dictionary<string, string> data = new Dictionary<string, string>()
                        {
                        {"phone_id", generateUUID(true) },
                        {"_csrftoken", resposeCookies["csrftoken"].Value},
                        {"username", this.username },
                        {"guid", this.uuid },
                        {"device_id", this.device_id},
                        {"password", this.password },
                        {"login_attempt_count", string.Format("{0}", attempt)}
                        };
                    var gs = generateSignature(JsonConvert.SerializeObject(data));
                    bool lreq = await SendRequest("accounts/login/", generateSignature(JsonConvert.SerializeObject(data)), true);
                    if (lreq)
                    {
                        isLoggedIn = true;
                        fail = false;
                        this.username_id = lastJson["logged_in_user"]["pk"].ToString();
                        this.rank_token = this.username_id + "_" + this.uuid;
                        this.token = resposeCookies["csrftoken"].Value; //5744828632
                        Console.Out.WriteLine("username_id:{0}\nrank_token:{1}\ntoken:{2}", username_id, rank_token, token);

                        await syncFeatures();
                        await autoCompleteUserList();
                        await timelineFeed();
                        await getv2Inbox();
                        await getRecentActivity();

                        loggedIn = true;
                        Console.Out.WriteLine("Logged in!");
                    }
                    else
                    {
                        if (lastResponse.StatusCode == (HttpStatusCode)400)
                        {
                            if (attempt < 3)
                            {
                                Console.Out.WriteLine("Relogin {0} time...", attempt);
                                await Task.Delay(500);
                                relogin(attempt + 1);
                            }
                            else
                                return;
                        }

                        fail = true;
                        loggedIn = false;
                    }
                }

            }
        }

        public async void login(bool force = false)
        {
            if(!this.isLoggedIn || force)
            {
                bool req = await SendRequest("si/fetch_headers/?challenge_type=signup&guid=" + generateUUID(false), null, true);
                if (req)
                {
                    Dictionary<string,string> data = new Dictionary<string, string>()
                        {
                        {"phone_id", generateUUID(true) },
                        {"_csrftoken", resposeCookies["csrftoken"].Value},
                        {"username", this.username },
                        {"guid", this.uuid },
                        {"device_id", this.device_id}, 
                        {"password", this.password },
                        {"login_attempt_count", "0" }
                        };
                    var gs = generateSignature(JsonConvert.SerializeObject(data));
                    bool lreq = await SendRequest("accounts/login/", generateSignature(JsonConvert.SerializeObject(data)), true);
                    if (lreq)
                    {
                        isLoggedIn = true;
                        fail = false;
                        this.username_id = lastJson["logged_in_user"]["pk"].ToString();
                        this.rank_token = this.username_id + "_" + this.uuid;
                        this.token = resposeCookies["csrftoken"].Value; //5744828632
                        Console.Out.WriteLine("username_id:{0}\nrank_token:{1}\ntoken:{2}", username_id, rank_token, token);

                        await syncFeatures();
                        await autoCompleteUserList();
                        await timelineFeed();
                        //await getv2Inbox();
                        await getRecentActivity();

                        loggedIn = true;
                        Console.Out.WriteLine("Logged in!");
                    }
                    else
                    {
                        if(lastResponse.StatusCode == (HttpStatusCode)400)
                        {
                            Console.Out.WriteLine("Relogin...");
                            await Task.Delay(500);
                            relogin(1);
                        }

                        fail = true;
                        loggedIn = false;
                    }
                }

            }

        }

        public async Task<bool> syncFeatures()
        {
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                {"_uuid", this.uuid },
                {"_uid", this.username_id },
                {"id", this.username_id },
                {"_csrftoken", this.token },
                {"experiments", EXPERIMENTS }
            };
            return await SendRequest("qe/sync/", generateSignature(JsonConvert.SerializeObject(data)));
        }

        public async Task<bool> autoCompleteUserList()
        {
            return await SendRequest("friendships/autocomplete_user_list/");
        }

        public async Task<bool> timelineFeed()
        {
            return await SendRequest("feed/timeline/");
        }

        public async Task<bool> getv2Inbox()
        {
            return await SendRequest("direct_v2/inbox/?");
        }

        public async Task<bool> getRecentActivity()
        {
            return await SendRequest("news/inbox/?");
        }

        public async Task<bool> getUserFollowers(string usernameId, string maxid = "")
        {
            if (maxid == "")
                return await SendRequest("friendships/" + usernameId + "/followers/?rank_token=" + rank_token);
            else
                return await SendRequest("friendships/" + usernameId + "/followers/?rank_token=" + rank_token + "&max_id=" + maxid);
        }

        public async Task<bool> getSelfUserFollowers()
        {
            return await getUserFollowers(username_id);
        }

        public async Task<bool> logout()
        {
            return await SendRequest("accounts/logout/");
        }

        //public async Task<bool> uploadPhoto(string photo, string caption = "", string upload_id = "", bool is_sidecar = false)
        //{
        //    if (string.IsNullOrEmpty(upload_id))
        //        upload_id = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds.ToString();
        //    Console.Out.WriteLine("upload photo, id = ", upload_id);

        //    Dictionary<string, string> data = new Dictionary<string, string>()
        //    {
        //        {"upload_id", upload_id },
        //        {"_uuid" , this.uuid},
        //        {"_csrftoken", this.token},
        //        {"image_compression","{\"lib_name\":\"jt\",\"lib_version\":\"1.3.0\",\"quality\":\"87\"}" },
        //        {"photo", "" }
        //    };
        //    if (is_sidecar)
        //        data["is_sidecar"] = "1";
        //    http.DefaultRequestHeaders.Add("X-IG-Connection-Type", "WIFI");
        //    http.DefaultRequestHeaders.Add("X-IG-Capabilities", "3Q4=");
        //    http.DefaultRequestHeaders.Add("Cookie2", "$Version=1");
        //    http.DefaultRequestHeaders.Add("Accept-Language", "en-US");
        //    http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
        //    http.DefaultRequestHeaders.Add("Content-type", "image");
        //    http.DefaultRequestHeaders.Add("Connection", "close");
        //    http.DefaultRequestHeaders.Add("User-Agent", USER_AGENT);
           
        //}

        public async Task<bool> removeProfilePicture()
        {
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "_uuid", this.uuid },
                { "_uid", this.username_id },
                {"_csrftoken", this.token }
            };
            return await SendRequest("accounts/remove_profile_picture/", generateSignature(JsonConvert.SerializeObject(data)));
        }

        //utils functions
        private string generateUUID(bool type)
        {
            var generated_uuid = System.Guid.NewGuid().ToString();
            if (type)
                return generated_uuid;
            else
                return generated_uuid.Replace("-", "");
        }

        private string generateSignature(string data, bool skip_quote = false)
        {
            string parsed_data;

            if (!skip_quote)
            {
                parsed_data = HttpUtility.UrlEncode(data);
            }
            else
            {
                parsed_data = data;
            }

            return "ig_sig_key_version=" + SIG_KEY_VERSION + "&signed_body=" + CalcHMACSHA256Hash(HexDecode(IG_SIG_KEY),data) + "." + parsed_data;
        }

        private string HexDecode(string hex)
        {
            var sb = new StringBuilder();
            for (int i = 0; i <= hex.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hex.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }

        private string CalcHMACSHA256Hash(string plaintext, string salt)
        {
            string result = "";
            var enc = Encoding.Default;
            byte[]
            baText2BeHashed = enc.GetBytes(plaintext),
            baSalt = enc.GetBytes(salt);
            System.Security.Cryptography.HMACSHA256 hasher = new HMACSHA256(baSalt);
            byte[] baHashedText = hasher.ComputeHash(baText2BeHashed);
            result = string.Join("", baHashedText.ToList().Select(b => b.ToString("x2")).ToArray());
            return result;
        }

        private string generateDeviceId(string seed)
        {
            string volatile_seed = "12345";
            var m = MD5.Create();
            var up = new byte[Encoding.UTF8.GetBytes(seed).Length + Encoding.UTF8.GetBytes(volatile_seed).Length];
            Encoding.UTF8.GetBytes(seed).CopyTo(up, 0);
            Encoding.UTF8.GetBytes(volatile_seed).CopyTo(up, Encoding.UTF8.GetBytes(seed).Length);
            m.ComputeHash(up);
            return "android-" + BitConverter.ToString(m.Hash).Replace("-","").Substring(0,16).ToLower();
        }

        private async Task<bool> SendRequest(string endpoint, string post = null, bool login = false)
        {
            var verify = false;

            if (!this.isLoggedIn && !login)
                throw new Exception("Not logged in!\n");

            HttpResponseMessage response;
            http.DefaultRequestHeaders.Connection.Add("close");
            http.DefaultRequestHeaders.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("en-US"));
            http.DefaultRequestHeaders.UserAgent.ParseAdd(USER_AGENT);

            while (true)
            {
                try
                {
                    try
                    {
                        await Console.Out.WriteLineAsync("POST: " + post);
                        var sc = new StringContent(post);
                        sc.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
                        sc.Headers.ContentType.CharSet = "utf-8";
                        response = await http.PostAsync(API_URL + endpoint, sc);
                    }
                    catch(ArgumentNullException)
                    {
                        await Console.Out.WriteLineAsync("GET: " + endpoint);
                        response = await http.GetAsync(API_URL + endpoint);
                    }
                    break;
                }
                catch (Exception e)
                {
                    await Console.Out.WriteLineAsync("Except on send request (wait for 60 seconds and resend): " + e.Message);
                    Thread.Sleep(60 * 1000);
                }
            }
            Console.Out.WriteLine((int)response.StatusCode);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                this.lastResponse = response;
                string ljson = await response.Content.ReadAsStringAsync();
                this.lastJson = JObject.Parse(ljson);
                Uri uri = new Uri(API_URL + endpoint);
                this.resposeCookies = cookies.GetCookies(uri);
                //this.lastJson = new JObject(response.Content.ToString());
                return true;
            }
            //if(response.StatusCode == (HttpStatusCode)429)
            //{
            //    Console.Out.WriteLine("Too many requests. Delay 5s\n");
            //    await Task.Delay(5000);
            //    return await SendRequest(endpoint, post, login);
            //}
            else
            {
                await Console.Out.WriteLineAsync("Request return " + response.StatusCode.ToString() + " error!");
                this.lastResponse = response;
                string ljson = await response.Content.ReadAsStringAsync();
                this.lastJson = JObject.Parse(ljson);
                Uri uri = new Uri(API_URL + endpoint);
                this.resposeCookies = cookies.GetCookies(uri);
                return false;
            }
        }

        //get-set
        public string getUsername
        {
            get
            {
                return this.username;
            }
        }

        public string getUsernameID
        {
            get
            {
                return this.username_id;
            }
        }

        public JObject LastJson { get => lastJson; set => lastJson = value; }
    }
}
